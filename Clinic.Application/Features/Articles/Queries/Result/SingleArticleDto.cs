using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Features.Articles.Queries.Result
{
    public sealed class SingleArticleDto
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;

        public string Slug { get; private set; } = string.Empty;

        public string Summary { get; private set; } = string.Empty;

        public string Content { get; private set; } = string.Empty;

        public ArticleStatus Status { get; private set; }
        public string? FeaturedImageUrl { get; private set; }
    }
}
