using Clinic.Domain.Common;

namespace Clinic.Domain.Entities.consultation
{
    public sealed class Consultation : BaseEntity<Guid>
    {
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Message { get; set; }

        public ConsultationStatus Status { get; set; }
    }
}
