using Clinic.Domain.Common;

namespace Clinic.Domain.Entities.artical
{
    public sealed class Article : BaseEntity<Guid>
    {
        public string Title { get; private set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Summary { get; private set; } = string.Empty;

        public string Content { get; private set; } = string.Empty;

        public string? FeaturedImageUrl { get; private set; }

        public ArticleStatus Status { get; private set; }

        public DateTime? PublishedAt { get; private set; }

        public Guid WriterId { get; private set; }


    }
}
