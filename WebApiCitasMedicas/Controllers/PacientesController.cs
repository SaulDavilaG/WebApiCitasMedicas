using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.DTOs;
using Microsoft.Extensions.Logging;
using WebApiCitasMedicas.Filtros;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [ResponseCache(Duration = 2)]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<PacientesController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public PacientesController(ApplicationDbContext dbContext, IMapper mapper, ILogger<PacientesController> logger, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Paciente>>> GetAll()
        {
            logger.LogInformation("Listado de pacientes");
            var pacientes = await dbContext.Pacientes.ToListAsync();
            return Ok(pacientes.Select(paciente => mapper.Map<PacienteDTO>(paciente)));
        }

        [HttpGet("MisCitas")]
        [ServiceFilter(typeof(AccionFiltro))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult<List<Cita>>> GetAllMy()
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var paciente = await dbContext.Pacientes.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var Citas = await dbContext.Citas.Where(x => x.PacienteID == paciente.Id).ToListAsync();
            var cita = await dbContext.Citas.AnyAsync(x => x.PacienteID == paciente.Id);

            if (!cita)
            {
                return BadRequest("No tiene citas registradas");
            }
            logger.LogInformation("Listado de Mis Citas");
            return Ok(Citas.Select(Cita => mapper.Map<CitaDTO>(Cita)));
        }

        [HttpGet("MiInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult<PacienteDTO>> GetMyInfo()
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var paciente = await dbContext.Pacientes.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            logger.LogInformation("Informacion desplegada exitosamente");
            return mapper.Map<PacienteDTO>(paciente);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult> Post(PacienteDTO pacienteDto)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var PacienteYaRegistrado = await dbContext.Pacientes.AnyAsync(x => x.UsuarioId == usuarioId);
            if (PacienteYaRegistrado)
            {
                return BadRequest("Ya has creado un perfil con esta cuenta");
            }

            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == pacienteDto.MedicoID);
            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            var medComparar = await dbContext.Pacientes.Where(x => x.MedicoID == pacienteDto.MedicoID).ToListAsync();

            var cont = 0;
            foreach (var item in medComparar)
            {
                cont++;
            }

            Console.WriteLine("Numero pacientes "+ cont);

            if (cont > 99)
            {
                return BadRequest("Limite de numero de pacientes alcanzado");
            }

            var paciente =mapper.Map<Paciente>(pacienteDto);
            paciente.UsuarioId = usuarioId;

            //dbContext.Add(paciente);

            logger.LogInformation("Registro de paciente exitoso");
            await dbContext.SaveChangesAsync();

            var pacientes = await dbContext.Pacientes.AnyAsync(x => x.UsuarioId == usuarioId);
            return Ok(pacientes);
        }

        [HttpPut("ModificarInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsPaciente")]
        public async Task<ActionResult> Put(PacienteDTOGet pacienteDtoGet)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == pacienteDtoGet.MedicoID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync(x => x.MedicoID == pacienteDtoGet.MedicoID && x.UsuarioId == usuarioId);

            var pacienteayuda = dbContext.Pacientes.AnyAsync(x => x.UsuarioId == usuarioId);

            if (!existeMedico)
            {
                return BadRequest("No existe el Medico");
            }
            if (!mismoMedico)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con el medico seleccionado");
            }

            var paciente = mapper.Map<Paciente>(pacienteDtoGet);
            paciente.UsuarioId = usuarioId;
            dbContext.Update(paciente);

            logger.LogInformation("Actualización de registro de paciente exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await dbContext.Pacientes.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound("No existe paciente con tal ID");
            }

            dbContext.Remove(new Paciente()
            {
                Id = id
            });
            logger.LogInformation("Eliminación de paciente exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
