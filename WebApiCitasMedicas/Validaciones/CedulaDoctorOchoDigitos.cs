using System.ComponentModel.DataAnnotations;

namespace WebApiCitasMedicas.Validaciones
{
    public class CedulaDoctorOchoDigitos : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            bool cedulaValida;

            var palabra = value.ToString();

            string patron = @"^\d{8}$";

            cedulaValida = System.Text.RegularExpressions.Regex.IsMatch(palabra, patron);

            if (cedulaValida)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("La cedula debe tener 8 digitos");
        }

    }
}
