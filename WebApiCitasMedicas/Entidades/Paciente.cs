using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiCitasMedicas.Entidades
{
    public class Paciente : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public float peso { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public float altura { get; set;}

        public string hist_medico { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string sexo { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int MedicoID { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (peso < 0)
            {
                yield return new ValidationResult("El atributo peso debe ser mayor que 0", new String[] { nameof(peso) });
            }

            if (altura > 2.5 || altura < 0)
            {
                yield return new ValidationResult("El atributo altura debe ser mayor que 0m y menor que 2.5m", new String[] { nameof(altura) });
            }

            if (sexo == "Masculino" || sexo == "Femenino"){}
            else
            {
                yield return new ValidationResult("El atributo sexo debe ser femenino o masculino", new String[] { nameof(sexo) });
            }
        }
    }
}
