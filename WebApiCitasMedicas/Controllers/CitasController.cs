using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Filtros;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.DTOs;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [ResponseCache(Duration = 2)]
    [Route("api/citas")]
    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<CitasController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public CitasController(ApplicationDbContext dbContext, IMapper mapper , ILogger<CitasController> logger, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [ServiceFilter(typeof(AccionFiltro))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult<List<Cita>>> GetAll()
        {
            logger.LogInformation("Listado de Citas");
            var Citas = await dbContext.Citas.ToListAsync();
            return Ok(Citas.Select(Cita=> mapper.Map<CitaDTO>(Cita)));
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult<CitaDTO>> GetByID(int id)
        {
            var cita = await dbContext.Citas.FirstOrDefaultAsync(x => x.Id == id);
            logger.LogInformation("Busqueda de cita por id exitosa");
            return mapper.Map<CitaDTO>(cita); 
        }

        [HttpGet("Nombre")] //Nadamás retorna la primera cita
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult<List<Cita>>> GetByName(string nombre)
        {
            if (nombre == null)
            {
                return NotFound();
            }
            var cita = await dbContext.Citas.Where(x => x.Paciente.nombre.Contains(nombre)).ToListAsync();
            logger.LogInformation("Busqueda de cita por nombre del paciente exitosa");
            return Ok(cita.Select(cita=> mapper.Map<CitaDTO>(cita)));
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Post(CitaDTO citaDto)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync( x => x.Id == citaDto.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync( x => x.Id == citaDto.PacienteID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync( x => x.MedicoID == citaDto.MedicoID && x.Id == citaDto.PacienteID );

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            if (!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            if (!mismoMedico)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con el medico seleccionado");
            }

            var cita = mapper.Map<Cita>(citaDto);
            dbContext.Add(cita);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Registro de cita exitoso");

            var citas = await dbContext.Citas.ToListAsync();
            return Ok(citas.Select(cita => mapper.Map<CitaDTO>(cita)));
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Put(CitaDTOGet citaDtoGet, int id)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == citaDtoGet.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync(x => x.Id == citaDtoGet.PacienteID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync(x => x.MedicoID == citaDtoGet.MedicoID && x.Id == citaDtoGet.PacienteID);

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            if(!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            if (citaDtoGet.Id != id)
            {
                return BadRequest("El ID de la cita no coincide en la URL ");
            }

            if (!mismoMedico)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con el medico seleccionado");
            }

            var cita = mapper.Map<Cita>(citaDtoGet);

            dbContext.Update(cita);

            logger.LogInformation("Actualización de registro de cita exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await dbContext.Citas.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound("No existe cita con tal ID");
            }

            dbContext.Remove(new Cita()
            {
                Id = id
            });
            logger.LogInformation("Eliminación de cita exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
