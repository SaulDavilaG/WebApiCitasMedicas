using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 

        }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cita>().HasOne( e => e.Medico).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Cita>().HasOne( e => e.Paciente).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
