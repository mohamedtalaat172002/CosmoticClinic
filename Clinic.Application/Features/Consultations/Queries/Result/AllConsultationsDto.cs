using Clinic.Domain.Entities.consultation;

namespace Clinic.Application.Features.Consultations.Queries.Result
{
    public record AllConsultationsDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Message { get; set; }

        public ConsultationStatus Status { get; set; }
    }
}
