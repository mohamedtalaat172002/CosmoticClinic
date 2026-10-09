namespace Clinic.Application.Features.Treatments.Queries.Result
{
    public class UpdateCreateSingleTreatmentDto
    {
        public Guid Id { get; set; }

        public string Slug { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<TreatmentImageDto> Images { get; set; } = new();
    }

    public class TreatmentImageDto
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public int DisplayOrder { get; set; }
    }
}
