using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Models
{
    public sealed record UpdateArticleCommand(
    Guid Id,
    string Title,
    string Summary,
    string Content,
    ArticleStatus Status
) : IRequest<Result<SingleArticleDto>>;
}
