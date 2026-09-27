namespace Clinic.Application.Features.Articles.Queries.Result
{
    public sealed class AllArticlesDto
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;

        public string Slug { get; private set; } = string.Empty;

        public string Summary { get; private set; } = string.Empty;

        public string? FeaturedImageUrl { get; private set; }
    }
}
