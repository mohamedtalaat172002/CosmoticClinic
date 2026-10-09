using Clinic.Domain.BaseEntity;
using Clinic.Domain.Entities.treatment;
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


        public ArticleStatus Status { get; set; }

        public DateTime? PublishedAt { get; set; }

        public ICollection<ArticleImage> Images { get; set; } = new List<ArticleImage>();
        public Guid? TreatmentId { get; set; }

        public Treatment? Treatment { get; set; }


    }
}
