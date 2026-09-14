using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Models
{
    public sealed record AddConsultationCommand(string FullName,

    string PhoneNumber,

    string? Message


    ) : IRequest<Result<SingleConsultationDto>>
    { }

}
