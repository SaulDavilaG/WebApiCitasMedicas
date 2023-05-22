using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.DTOs;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    //[Authorize]
    public class PacientesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<PacientesController> logger;
        private readonly IMapper mapper;

        public PacientesController(ApplicationDbContext dbContext, IMapper mapper, ILogger<PacientesController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paciente>>> GetAll()
        {
            logger.LogInformation("Listado de pacientes");
            var pacientes = await dbContext.Pacientes.ToListAsync();
            return Ok(pacientes.Select(paciente => mapper.Map<PacienteDTO>(paciente)));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PacienteDTO>> GetByID(int id)
        {
            var paciente = await dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);
            logger.LogInformation("Busqueda de paciente por id exitosa");
            return mapper.Map<PacienteDTO>(paciente);
         
        }

        [HttpPost]
        public async Task<ActionResult> Post(PacienteDTO pacienteDto)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == pacienteDto.MedicoID);

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            var paciente=mapper.Map<Paciente>(pacienteDto); 
            dbContext.Add(paciente);
            logger.LogInformation("Registro de paciente exitoso");
            await dbContext.SaveChangesAsync();

            var pacientes = await dbContext.Pacientes.ToListAsync();
            return Ok(pacientes.Select(paciente => mapper.Map<PacienteDTO>(paciente)));

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(PacienteDTOGet pacienteDtoGet, int id)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == pacienteDtoGet.MedicoID);
            var mismoMedico = await dbContext.Pacientes.AnyAsync(x => x.MedicoID == pacienteDtoGet.MedicoID && x.Id == pacienteDtoGet.Id);

            if (!existeMedico)
            {
                return BadRequest("No existe el Medico");
            }

            if (pacienteDtoGet.Id != id)
            {
                return BadRequest("El ID del paciente no coincide en la URL ");
            }
            if (!mismoMedico)
            {
                return BadRequest("El paciente seleccionado no tiene relacion con el medico seleccionado");
            }

            var paciente = mapper.Map<Paciente>(pacienteDtoGet);

            dbContext.Update(paciente);

            logger.LogInformation("Actualización de registro de paciente exitoso");
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
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
