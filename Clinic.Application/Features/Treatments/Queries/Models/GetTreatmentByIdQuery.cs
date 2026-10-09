using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Queries.Models
{
    public sealed record GetTreatmentByIdQuery(Guid Id)
        : IRequest<Result<AdminTreatmentDto>>;
}