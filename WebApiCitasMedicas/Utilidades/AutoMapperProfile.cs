using AutoMapper;
using WebApiCitasMedicas.DTOs;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Paciente, PacienteDTO>();
            CreateMap<PacienteDTO, Paciente>();
            CreateMap<Paciente, PacienteDTOGet>();
            CreateMap<PacienteDTOGet, Paciente>();

            CreateMap<Medico, MedicoDTO>();
            CreateMap<MedicoDTO, Medico>();
            CreateMap<Medico, MedicoDTOGet>();
            CreateMap<MedicoDTOGet, Medico>();

            CreateMap<Cita, CitaDTO>();
            CreateMap<CitaDTO, Cita>();
            CreateMap<Cita, CitaDTOGet>();
            CreateMap<CitaDTOGet, Cita>();
        }
    }
}
