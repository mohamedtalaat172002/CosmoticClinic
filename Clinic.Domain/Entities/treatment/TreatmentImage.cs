using Clinic.Domain.BaseEntity;
using Clinic.Domain.Entities.treatment;

namespace Clinic.Domain.Entities
{
    public class TreatmentImage : ImageBase
    {
        public Guid TreatmentId { get; private set; }

        public Treatment Treatment { get; private set; } = null!;

        private TreatmentImage()
        {
        }

        public TreatmentImage(
            Guid treatmentId,
            string url,
            string publicId,
            int displayOrder = 0,
            bool isPrimary = false)
            : base(
                url,
                publicId,
                displayOrder,
                isPrimary)
        {
            TreatmentId = treatmentId;
        }
    }
}
