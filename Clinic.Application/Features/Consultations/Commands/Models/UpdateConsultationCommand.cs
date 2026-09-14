using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Domain.Entities.consultation;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Models
{
    public sealed record UpdateConsultationCommand
        (
        Guid Id,
        string FullName,

    string PhoneNumber,

    string? Message,

    ConsultationStatus Status)
        : IRequest<Result<SingleConsultationDto>>
    {
    }
}
