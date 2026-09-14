using AutoMapper;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Domain.Entities.consultation;

namespace Clinic.Application.Mappings.ConsultationMapping
{
    public partial class ConsultationProfile : Profile
    {
        public void GetSingleConultaionByIdMapping()
        {
            CreateMap<Consultation, SingleConsultationDto>();
        }
    }
}
