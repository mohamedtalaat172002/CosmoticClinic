using Clinic.Domain.BaseEntity;
using Clinic.Domain.Entities.artical;

namespace Clinic.Domain.Entities.treatment
{
    public sealed class Treatment : SluggableBaseEntity<Guid>
    {
        public Treatment()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<TreatmentImage> Images { get; set; } = new List<TreatmentImage>();
        public ICollection<Article> Articles { get; set; }
     = new List<Article>();
    }
}
