namespace Clinic.Application.Features.ClinicSetting.Queries.Result
{
    public record ClinicSettingDto
    {
        public Guid Id { get; init; }
        public string? ClinicName { get; init; }
        public string DoctorName { get; init; }
        public string? Specialization { get; init; }
        public string? Description { get; init; }
        public string? WhatsAppNumber { get; init; }
        public string? Email { get; init; }
        public string Address { get; init; }
        public string? LogoUrl { get; init; }
        public string? HeroImageUrl { get; init; }
        public string? FacebookUrl { get; init; }
        public string? InstagramUrl { get; init; }
    }
}
