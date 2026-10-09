using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Articles.Queries.Models
{
    public sealed record GetArticleByIdQuery(Guid Id)
        : IRequest<Result<AdminArticleDto>>;
}