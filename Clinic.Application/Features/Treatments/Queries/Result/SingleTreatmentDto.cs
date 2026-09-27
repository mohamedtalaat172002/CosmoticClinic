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
        public string? ImageUrl { get; set; }

        public PagedResult<AllArticlesDto> Articles { get; set; }

    }
}
