using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Articles.Queries.Result;

namespace Clinic.Application.Features.Treatments.Queries.Result
{
    public sealed class AdminTreatmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public PagedResult<AllArticlesDto> Articles { get; set; }
        public List<TreatmentImageDto> Images { get; set; } = new();
    }
}