namespace Clinic.Api.RequestModels.ClinicSettingRequestModels
{
    public class UpdateClinicSettingRequest
    {
        public string? ClinicName { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? Description { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
