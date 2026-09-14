using Clinic.Application.Common.ResponseFormat;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Models
{
    public sealed record DeleteConsultationCommand(Guid Id) : IRequest<Result<bool>>;
}
