using System.ComponentModel.DataAnnotations;

namespace WebApiCitasMedicas.DTOs
{
    public class CitaDTO
    {
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_cita { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo medico es requerido")]
        public int MedicoID { get; set; }

        [Required(ErrorMessage = "El campo paciente es requerido")]
        public int PacienteID { get; set; }
    }
}



