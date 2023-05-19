namespace WebApiCitasMedicas.Entidades
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime fecha_cita  { get; set; }
        public string descripcion { get; set; }
        public int PacienteID{ get; set;}
        public Paciente Paciente { get; set;}
    }
}
