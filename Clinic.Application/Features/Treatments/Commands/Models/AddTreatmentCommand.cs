using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Models
{
    public sealed record AddTreatmentCommand(
     string Name,
     string Description,
     string? ImageUrl
 ) : IRequest<Result<SingleTreatmentDto>>;
}
