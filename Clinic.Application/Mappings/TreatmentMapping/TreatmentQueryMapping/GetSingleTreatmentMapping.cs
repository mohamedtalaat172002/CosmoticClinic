using AutoMapper;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Domain.Entities.treatment;

namespace Clinic.Application.Mappings.TreatmentMapping
{
    public partial class TreatmentProfile : Profile
    {
        public void GetSingleTreatmentMapping()
        {
            CreateMap<Treatment, SingleTreatmentDto>();

        }
    }

}
