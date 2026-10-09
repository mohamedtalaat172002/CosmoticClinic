using Clinic.Domain.BaseEntity;
using Clinic.Domain.Entities.clinicSettings;

namespace Clinic.Domain.Entities
{
    public class ClinicSettingImage : ImageBase
    {
        public Guid ClinicSettingId { get; private set; }

        public ClinicSettings ClinicSetting { get; private set; } = null!;

        private ClinicSettingImage()
        {
        }

        public ClinicSettingImage(
            Guid clinicSettingId,
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
            ClinicSettingId = clinicSettingId;
        }
    }
}
