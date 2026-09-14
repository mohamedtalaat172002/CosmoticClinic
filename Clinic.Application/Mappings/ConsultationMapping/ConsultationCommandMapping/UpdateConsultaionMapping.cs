using AutoMapper;

namespace Clinic.Application.Mappings.ConsultationMapping
{
    public partial class ConsultationProfile : Profile
    {
        public void UpdateConsultaionMapping()
        {
            CreateMap<
                Clinic.Domain.Entities.consultation.Consultation,
                Clinic.Application.Features.Consultations.Commands.Models.UpdateConsultationCommand
            >().ReverseMap();
        }
    }
}
