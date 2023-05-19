namespace WebApiCitasMedicas.Entidades
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime Fecha_cita  { get; set; }
        public string Descripcion { get; set; }
        public int MedicoID { get; set;}
        public Medico Medico { get; set;}
        public int PacienteID{ get; set;}
        public Paciente Paciente { get; set;}
    }
}
