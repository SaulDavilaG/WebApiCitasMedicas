using System.ComponentModel.DataAnnotations;

namespace WebApiCitasMedicas.DTOs
{
    public class CitaDTOGet
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime Fecha_cita { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo medico es requerido")]
        public int MedicoID { get; set; }

        [Required(ErrorMessage = "El campo paciente es requerido")]
        public int PacienteID { get; set; }
    }
}
