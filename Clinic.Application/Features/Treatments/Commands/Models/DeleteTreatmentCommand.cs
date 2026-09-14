using Clinic.Application.Common.ResponseFormat;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Models
{
    public sealed record DeleteTreatmentCommand(
    Guid Id
) : IRequest<Result<bool>>;
}
