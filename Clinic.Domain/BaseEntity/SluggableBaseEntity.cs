using Clinic.Domain.Common;

namespace Clinic.Domain.BaseEntity
{
    public abstract class SluggableBaseEntity<T> : BaseEntity<T>
    {
        public string Slug { get; set; } = string.Empty;
    }
}
