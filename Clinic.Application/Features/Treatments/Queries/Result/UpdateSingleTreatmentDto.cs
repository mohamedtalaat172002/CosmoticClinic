namespace Clinic.Application.Features.Treatments.Queries.Result
{
    public record UpdateSingleTreatmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
