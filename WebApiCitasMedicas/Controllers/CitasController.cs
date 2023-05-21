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

        public CitasController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cita>>> GetAll()
        {
            return await dbContext.Citas.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cita>> GetByID(int id)
        {
            return await dbContext.Citas.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cita cita)
        {
            var existeMedico = await dbContext.Medicos.AnyAsync(x => x.Id == cita.MedicoID);
            var existePaciente = await dbContext.Pacientes.AnyAsync(x => x.Id == cita.PacienteID);

            if (!existeMedico)
            {
                return BadRequest("No existe el medico");
            }

            if (!existePaciente)
            {
                return BadRequest("No existe el paciente");
            }

            dbContext.Add(cita);
            await dbContext.SaveChangesAsync();
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
            await dbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
