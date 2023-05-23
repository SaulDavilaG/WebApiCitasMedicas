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
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x=>x.UsuarioId == usuarioId);
            logger.LogInformation("Listado de Citas");
            var Citas = await dbContext.Citas.Where(x => x.MedicoID == medico.Id).ToListAsync();

            return Ok(Citas.Select(Cita => mapper.Map<CitaDTO>(Cita)));
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult<CitaDTO>> GetByID(int id)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var paciente = await dbContext.Pacientes.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var cita = await dbContext.Citas.FirstOrDefaultAsync(x => x.Id == id && x.PacienteID == paciente.Id);
            logger.LogInformation("Busqueda de cita por id exitosa");
            return mapper.Map<CitaDTO>(cita); 
        }
        /* Este método es más general, y permite buscar citas de cualquier usuario solo con su nombre
        [HttpGet("Nombre")]
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
        */

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Post(CitaDTO citaDto)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            var cita = mapper.Map<Cita>(citaDto);
            cita.MedicoID = medico.Id;

            var existeMedico = await dbContext.Medicos.AnyAsync( x => x.Id == cita.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync( x => x.Id == cita.PacienteID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync( x => x.MedicoID == cita.MedicoID && x.Id == cita.PacienteID );

            if (!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            if (!mismoMedico)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con el medico seleccionado");
            }

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
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == citaDtoGet.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync(x => x.Id == citaDtoGet.PacienteID);
            var miPaciente = await dbContext.Pacientes.AnyAsync(x => x.MedicoID == medico.Id && x.Id == citaDtoGet.PacienteID);

            if(!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            if (citaDtoGet.Id != id)
            {
                return BadRequest("El ID de la cita no coincide en la URL ");
            }

            if (!miPaciente)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con usted");
            }

            var cita = mapper.Map<Cita>(citaDtoGet);
            cita.MedicoID = medico.Id;

            dbContext.Update(cita);

            logger.LogInformation("Actualización de registro de cita exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Delete(int id)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            var exists = await dbContext.Citas.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound("No existe cita con tal ID");
            }
            var tucita = await dbContext.Citas.AnyAsync(x => x.Id == id && x.MedicoID == medico.Id);

            if (!tucita)
            {
                return NotFound("Imposible eliminar la cita");
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
