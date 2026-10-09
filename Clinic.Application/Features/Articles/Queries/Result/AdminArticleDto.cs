using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Features.Articles.Queries.Result
{
    public sealed class AdminArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ArticleStatus Status { get; set; }
        public Guid? TreatmentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<ArticleImageDto> Images { get; set; } = new();
    }
}