using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiCitasMedicas.Validaciones;

namespace WebApiCitasMedicas.Entidades
{
    public class Medico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre_med { get; set;}

        [Required(ErrorMessage = "El campo Cedula es requerido")]
        [CedulaDoctorOchoDigitos]
        public string Cedula { get; set;}

        public List<Paciente> pacientes { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }
    }
}
