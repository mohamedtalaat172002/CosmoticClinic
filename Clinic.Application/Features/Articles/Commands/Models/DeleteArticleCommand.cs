using Clinic.Application.Common.ResponseFormat;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Models
{
    public sealed record DeleteArticleCommand(
    Guid Id
) : IRequest<Result<bool>>;
}
