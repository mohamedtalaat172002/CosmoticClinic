using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Consultations.Queries.Models
{
    public sealed record GetConsultationByIdQuery(Guid id)
        : IRequest<Result<SingleConsultationDto>>
    {
    }
}
