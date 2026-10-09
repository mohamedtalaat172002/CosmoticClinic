using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Domain.Entities.artical;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetArticleBySlugQueryHandler
        : IRequestHandler<GetArticleBySlugQuery, Result<SingleArticleDto>>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticleBySlugQueryHandler(IArticleReadRepository articleReadRepository)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<Result<SingleArticleDto>> Handle(
            GetArticleBySlugQuery request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository
                .GetAll()
                .Where(a => a.Slug == request.Slug
                            && !a.IsDeleted
                            && a.Status == ArticleStatus.Published)
                .Select(a => new SingleArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Slug = a.Slug,
                    Summary = a.Summary,
                    Content = a.Content,
                    Status = a.Status,
                    FeaturedImageUrl = a.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.Url)
                        .FirstOrDefault(),
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
                return Result<SingleArticleDto>.Failure(
                    "Article not found",
                    ErrorType.NotFound);
            }

            return Result<SingleArticleDto>.Success(article);
        }
    }
}