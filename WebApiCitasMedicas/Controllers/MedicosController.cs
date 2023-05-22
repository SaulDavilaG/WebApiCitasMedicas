using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    //[Authorize]
    public class MedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<MedicosController> logger;

        public MedicosController(ApplicationDbContext context, ILogger<MedicosController> logger)
        {
            this.dbContext = context;
            this.logger = logger;
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
        //[ResponseCache(Duration =10)]
        public async Task<ActionResult<List<Medico>>> GetAll()
        {
            logger.LogInformation("Listado de médicos");
            return await dbContext.Medicos.Include(x => x.pacientes).ToListAsync();
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Medico>> Get(string nombre)
        {
            var medico = await dbContext.Medicos.FirstOrDefaultAsync(x => x.Nombre_med.Equals(nombre));

            if (nombre == null) 
            { 
                return NotFound();
            }

            logger.LogInformation("Busqueda de medico por nombre exitosa");
            return medico;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Medico medico)
        {
            dbContext.Add(medico);
            logger.LogInformation("Registro de médico exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Medico medico, int id)
        {
            if (medico.Id != id)
            {
                return BadRequest("El ID del medico no coincide con el establecido en la url");
            }

            dbContext.Update(medico);

            logger.LogInformation("Actualización de registro exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
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
