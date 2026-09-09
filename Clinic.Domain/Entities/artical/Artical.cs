using Clinic.Domain.BaseEntity;
namespace Clinic.Domain.Entities.artical
{
    public sealed class Article : SluggableBaseEntity<Guid>
    {
        public Article()
        {
            Id = Guid.NewGuid();
        }
        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string? FeaturedImageUrl { get; set; }

        public ArticleStatus Status { get; set; }

        public DateTime? PublishedAt { get; set; }

        public Guid WriterId { get; set; }


    }
}
