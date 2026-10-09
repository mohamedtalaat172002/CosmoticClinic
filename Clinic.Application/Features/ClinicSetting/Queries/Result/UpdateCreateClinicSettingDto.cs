namespace Clinic.Application.Features.ClinicSetting.Queries.Result
{
    public class UpdateCreateClinicSettingDto
    {
        public Guid Id { get; set; }
        public string? ClinicName { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? Description { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public List<ClinicSettingImageDto> Images { get; set; } = new();
    }

    public class ClinicSettingImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
