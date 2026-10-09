using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Articles.Queries.Result;

namespace Clinic.Application.Features.Treatments.Queries.Result
{
    public class SingleTreatmentDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl => Images.FirstOrDefault(x => x.IsPrimary)?.Url
                                ?? Images.FirstOrDefault()?.Url;

        public List<TreatmentImageDto> Images { get; set; } = new();

        public PagedResult<AllArticlesDto> Articles { get; set; }
    }
}
