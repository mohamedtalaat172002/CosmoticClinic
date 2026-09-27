namespace Clinic.Application.Features.Treatments.Queries.Result
{
    public class AllTreatmentsDto
    {
        public Guid Id { get; private set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string? ImageUrl { get; private set; }
    }
}
