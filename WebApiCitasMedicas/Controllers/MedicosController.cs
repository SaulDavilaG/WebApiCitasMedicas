using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.DTOs;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.Migrations;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    //[Authorize]
    public class MedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<MedicosController> logger;
        private readonly IMapper mapper;

        public MedicosController(ApplicationDbContext context, ILogger<MedicosController> logger, IMapper mapper)
        {
            this.dbContext = context;
            this.logger = logger;
            this.mapper = mapper;
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

            var medicos = await dbContext.Medicos.ToListAsync();
            return Ok(medicos.Select(medico => mapper.Map<MedicoDTO>(medico)));
        }

        [HttpGet("{nombre}")]
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
        public async Task<ActionResult> Post(MedicoDTO medicoDto) 
        {
            var medico = mapper.Map<Medico>(medicoDto);
            dbContext.Add(medico);
            logger.LogInformation("Registro de médico exitoso");
            await dbContext.SaveChangesAsync();

            var medicos = await dbContext.Medicos.ToListAsync();
            return Ok(medicos.Select(medico=>mapper.Map<MedicoDTO>(medico)));       
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(MedicoDTOGet medicoDtoGet, int id)
        {

            if (medicoDtoGet.Id != id)
            {
                return BadRequest("El ID del medico no coincide con el establecido en la url");
            }
            var medico = mapper.Map<Medico>(medicoDtoGet);
            dbContext.Update(medico);

            logger.LogInformation("Actualización de registro exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

 //pues no se osea tu dices de q pueda cambiar su cedula, pero esa madre no cambia no?
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
