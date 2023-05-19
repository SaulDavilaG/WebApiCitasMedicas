namespace WebApiCitasMedicas.Entidades
{
    public class Paciente
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public float peso { get; set;}
        public float altura { get; set;}
        public string hist_medico { get; set;}
        public string sexo { get; set;}
        public int MedicoID { get; set; }
        public Medico Medico { get; set;}
    }
}
