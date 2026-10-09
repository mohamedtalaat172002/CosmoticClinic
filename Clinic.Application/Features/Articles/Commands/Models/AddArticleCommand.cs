using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Models
{
    public sealed record AddArticleCommand(
        string Title,
        string Summary,
        string Content,
        ArticleStatus Status,
        Guid? TreatmentId,
        IReadOnlyCollection<ArticleImageUpload> Images
    ) : IRequest<Result<UpdateCreateSingleArticleDto>>;

    public sealed record ArticleImageUpload(
        Stream FileStream,
        string FileName
    );
}
