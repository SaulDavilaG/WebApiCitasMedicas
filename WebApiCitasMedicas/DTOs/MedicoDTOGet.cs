using System.ComponentModel.DataAnnotations;
using WebApiCitasMedicas.Validaciones;

namespace WebApiCitasMedicas.DTOs
{
    public class MedicoDTOGet
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre_med { get; set; }

        [Required(ErrorMessage = "El campo Cedula es requerido")]
        [CedulaDoctorOchoDigitos]
        public string Cedula { get; set; }
    }
}
