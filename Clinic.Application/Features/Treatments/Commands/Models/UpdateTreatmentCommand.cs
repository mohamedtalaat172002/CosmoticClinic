using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Models
{
    public sealed record UpdateTreatmentCommand(
    Guid Id,
    string Name,
    string Description,
    string? ImageUrl
) : IRequest<Result<UpdateSingleTreatmentDto>>;
}
