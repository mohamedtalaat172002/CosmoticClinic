namespace Clinic.Application.Features.ClinicSetting.Queries.Result
{
    public record ClinicSettingDto
    {
        public Guid Id { get; init; }
        public string? ClinicName { get; init; }
        public string DoctorName { get; init; } = string.Empty;
        public string? Specialization { get; init; }
        public string? Description { get; init; }
        public string? WhatsAppNumber { get; init; }
        public string? Email { get; init; }
        public string Address { get; init; } = string.Empty;
        public string? FacebookUrl { get; init; }
        public string? InstagramUrl { get; init; }

        // URL of the primary image (null if there are no images).
        public string? HeroImageUrl { get; init; }

        // All images, ordered by DisplayOrder (the primary image is first).
        public List<ClinicSettingImageDto> Images { get; init; } = new();
    }
}