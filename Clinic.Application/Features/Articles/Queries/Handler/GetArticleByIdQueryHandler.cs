using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetArticleByIdQueryHandler
        : IRequestHandler<GetArticleByIdQuery, Result<AdminArticleDto>>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticleByIdQueryHandler(IArticleReadRepository articleReadRepository)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<Result<AdminArticleDto>> Handle(
            GetArticleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository
                .GetAll()
                .Where(a => a.Id == request.Id)
                .Select(a => new AdminArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Slug = a.Slug,
                    Summary = a.Summary,
                    Content = a.Content,
                    Status = a.Status,
                    TreatmentId = a.TreatmentId,
                    IsDeleted = a.IsDeleted,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Images = a.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ArticleImageDto
                        {
                            Id = i.Id,
                            Url = i.Url,
                            IsPrimary = i.IsPrimary,
                            DisplayOrder = i.DisplayOrder
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (article is null)
            {
                return Result<AdminArticleDto>.Failure(
                    "Article not found.",
                    ErrorType.NotFound);
            }

            return Result<AdminArticleDto>.Success(article);
        }
    }
}