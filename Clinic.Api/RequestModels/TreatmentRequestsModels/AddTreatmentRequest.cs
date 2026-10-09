namespace Clinic.Api.RequestModels.TreatmentRequestsModels
{
    public class AddTreatmentRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<IFormFile> Images { get; set; } = new();
    }
}

