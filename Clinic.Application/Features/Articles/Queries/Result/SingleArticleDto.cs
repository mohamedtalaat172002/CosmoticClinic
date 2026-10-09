using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Features.Articles.Queries.Result
{
    public sealed class SingleArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ArticleStatus Status { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public List<ArticleImageDto> Images { get; set; } = new();
    }
}