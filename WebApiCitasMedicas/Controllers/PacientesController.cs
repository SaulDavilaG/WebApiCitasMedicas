using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public PacientesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paciente>>> GetAll()
        {
            return await dbContext.Pacientes.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Paciente>> GetByID(int id)
        { 
            return await dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Paciente paciente)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == paciente.MedicoID);

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            dbContext.Add(paciente);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Paciente paciente, int id)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == paciente.MedicoID);

            if (!existeMedico)
            {
                return BadRequest("No existe el Medico");
            }

            if (paciente.Id != id)
            {
                return BadRequest("El ID del medico no coincide en la URL ");
            }

            dbContext.Update(paciente);
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
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
