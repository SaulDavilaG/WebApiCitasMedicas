using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/citas")]
    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<CitasController> logger;

        public CitasController(ApplicationDbContext dbContext, ILogger<CitasController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cita>>> GetAll()
        {
            logger.LogInformation("Listado de Citas");
            return await dbContext.Citas.Include(x => x.Paciente).ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Cita>> GetByID(int id)
        {
            logger.LogInformation("Busqueda de cita por id exitosa");
            return await dbContext.Citas.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("{Nombre}")]
        public async Task<ActionResult<Cita>> GetByName(string nombre)
        {
            if (nombre == null)
            {
                return NotFound();
            }
            logger.LogInformation("Busqueda de cita por nombre exitosa");
            return await dbContext.Citas.FirstOrDefaultAsync(x => x.Paciente.nombre == nombre);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cita cita)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync( x => x.Id == cita.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync( x => x.Id == cita.PacienteID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync( x => x.MedicoID == cita.MedicoID && x.Id == cita.PacienteID );

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

            dbContext.Add(cita);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Registro de cita exitoso");
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Cita cita, int id)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == cita.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync(x => x.Id == cita.PacienteID);

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            if(!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            if (cita.Id != id)
            {
                return BadRequest("El ID de la cita no coincide en la URL ");
            }

            dbContext.Update(cita);
            logger.LogInformation("Actualización de registro de cita exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
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
