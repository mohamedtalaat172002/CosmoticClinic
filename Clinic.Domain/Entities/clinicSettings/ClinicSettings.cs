using Clinic.Domain.Common;

namespace Clinic.Domain.Entities.clinicSettings
{
    public sealed class ClinicSettings : BaseEntity<Guid>
    {
        public string? ClinicName { get; set; }
        public string DoctorName { get; set; } = null!;
        public string? Specialization { get; set; }
        public string? Description { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; } = null!;

        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public ICollection<ClinicSettingImage> Images { get; set; } = new List<ClinicSettingImage>();
        public ClinicSettings()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        }
    }
}
