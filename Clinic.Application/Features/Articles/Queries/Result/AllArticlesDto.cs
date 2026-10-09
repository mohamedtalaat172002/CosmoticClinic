using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Features.Articles.Queries.Result
{
    public sealed class AllArticlesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ArticleStatus Status { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public ArticleImageDto? FeaturedImageUrl { get; set; }
    }
}