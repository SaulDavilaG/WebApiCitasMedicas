using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.DTOs;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.Filtros;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [ResponseCache(Duration = 2)]
    [Route("api/medicos")]
    [ServiceFilter(typeof(AccionFiltro))]
    public class MedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<MedicosController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public MedicosController(ApplicationDbContext context, ILogger<MedicosController> logger, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.dbContext = context;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        /*
        [HttpGet]
        public ActionResult<List<Medico>> Get() {
            return new List<Medico>(){
             new Medico() { Id = 1, Nombre_med = "Saul" },
             new Medico() { Id = 2, Nombre_med = "Luis" },
            };
        }*/

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult<List<Medico>>> GetAll()
        {
            logger.LogInformation("Listado de médicos");

            var medicos = await dbContext.Medicos.ToListAsync();
            //var medicoss = dbContext.Medicos.ToListAsync();
            //logger.LogInformation(medicoss.ToString());
            return Ok(medicos.Select(medico => mapper.Map<MedicoDTO>(medico)));
        }

        [HttpGet("MisPacientes")]
        [ServiceFilter(typeof(AccionFiltro))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult<List<Paciente>>> GetAllMy()
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var Pacientes = await dbContext.Pacientes.Where(x => x.MedicoID == medico.Id).ToListAsync();
            var Paciente = await dbContext.Pacientes.AnyAsync(x => x.MedicoID == medico.Id);

            if (!Paciente)
            {
                return BadRequest("No tiene pacientes registrados");
            }

            logger.LogInformation("Listado de Mis Pacientes");
            return Ok(Pacientes.Select(Paciente => mapper.Map<PacienteDTO>(Paciente)));
        }

        [HttpGet("{nombre}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult<MedicoDTO>> Get(string nombre)
        {

            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.Nombre_med.Equals(nombre));

            if (nombre == null) 
            { 
                return NotFound();
            }

            logger.LogInformation("Busqueda de medico por nombre exitosa");
            return mapper.Map<MedicoDTO>(medico);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Post(MedicoDTO medicoDto) 
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var MedicoYaRegistrado = await dbContext.Medicos.AnyAsync(x => x.UsuarioId == usuarioId);

            if (MedicoYaRegistrado)
            {
                return BadRequest("Ya has creado un perfil con esta cuenta");
            }

            var medico = mapper.Map<Medico>(medicoDto);
            medico.UsuarioId = usuarioId;

            dbContext.Add(medico);

            logger.LogInformation("Registro de médico exitoso");
            await dbContext.SaveChangesAsync();

            var medicos = await dbContext.Medicos.ToListAsync();
            return Ok(medicos.Select(medico=>mapper.Map<MedicoDTO>(medico)));       
        }


        [HttpPut("ModificarInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]
        public async Task<ActionResult> Put(MedicoDTOGet medicoDtoGet)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var medico = mapper.Map<Medico>(medicoDtoGet);
            medico.UsuarioId = usuarioId;

            dbContext.Update(medico);

            logger.LogInformation("Actualización de registro exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await dbContext.Medicos.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound("No existe registro con tal ID");
            }

            dbContext.Remove(new Medico()
            {
                Id = id
            });

            logger.LogInformation("Eliminación de médico exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
