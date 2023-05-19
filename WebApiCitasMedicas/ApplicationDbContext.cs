using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;
using WebApiCitasMedicas.Migrations;

namespace WebApiCitasMedicas
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Citas>()
                .HasKey(al => new { al.MedicoID, al.PacienteID });
        }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }

        

    }
}
