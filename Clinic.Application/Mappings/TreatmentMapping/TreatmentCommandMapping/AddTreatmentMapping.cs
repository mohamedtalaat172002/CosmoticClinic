using AutoMapper;

namespace Clinic.Application.Mappings.TreatmentMapping
{
    public partial class TreatmentProfile : Profile
    {
        public void AddTreatmentMapping()
        {
            CreateMap<Clinic.Domain.Entities.treatment.Treatment, Clinic.Application.Features.Treatments.Commands.Models.AddTreatmentCommand>().ReverseMap();

        }
    }

}
