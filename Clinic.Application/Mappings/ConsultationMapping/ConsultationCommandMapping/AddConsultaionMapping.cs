using AutoMapper;

namespace Clinic.Application.Mappings.ConsultationMapping
{
    public partial class ConsultationProfile : Profile
    {
        public void AddConsultaionMapping()
        {
            CreateMap<
                    Clinic.Domain.Entities.consultation.Consultation,
                    Clinic.Application.Features.Consultations.Commands.Models.AddConsultationCommand
                >().ReverseMap();
        }
    }
}
