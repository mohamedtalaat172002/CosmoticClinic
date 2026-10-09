using AutoMapper;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Domain.Entities.treatment;

namespace Clinic.Application.Mappings.TreatmentMapping
{
    public partial class TreatmentProfile : Profile
    {
        public void GetAllTreatmentsMapping()
        {
            CreateMap<Treatment, AllTreatmentsDto>().
                ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Images
                .Where(x => x.IsPrimary)
                .Select(x => x.Url).FirstOrDefault()));
        }
    }


}
