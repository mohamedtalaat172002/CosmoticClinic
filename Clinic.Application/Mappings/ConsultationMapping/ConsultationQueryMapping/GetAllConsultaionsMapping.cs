using AutoMapper;

namespace Clinic.Application.Mappings.ConsultationMapping
{
    public partial class ConsultationProfile : Profile
    {

        public void GetAllConsultaionsMapping()
        {
            CreateMap<Domain.Entities.consultation.Consultation, Features.Consultations.Queries.Result.AllConsultationsDto>();
        }
    }
}
