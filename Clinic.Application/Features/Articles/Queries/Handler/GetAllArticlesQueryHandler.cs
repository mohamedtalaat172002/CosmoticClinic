using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetAllArticlesQueryHandler
        : IRequestHandler<GetAllArticlesQuery, Result<PagedResult<AllArticlesDto>>>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetAllArticlesQueryHandler(IArticleReadRepository articleReadRepository)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<Result<PagedResult<AllArticlesDto>>> Handle(
            GetAllArticlesQuery request,
            CancellationToken cancellationToken)
        {
            // Deleted articles never enter the count or the list.
            // Same filtered query feeds both => TotalCount always matches the list.
            var filtered = _articleReadRepository.ApplyFilters(
                _articleReadRepository
                    .GetAll()
                    .Where(a => !a.IsDeleted),
                request);

            var totalCount = await filtered.CountAsync(cancellationToken);

            // Order BEFORE paging; ThenBy(Id) keeps the order stable between pages.
            var ordered = filtered
                .OrderByDescending(a => a.CreatedAt)
                .ThenBy(a => a.Id);

            // Only the columns needed + the primary image URL (subquery, no Include).
            var items = await _articleReadRepository
                .ApplyPaging(ordered, request)
                .Select(a => new AllArticlesDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Slug = a.Slug,
                    Summary = a.Summary,
                    Status = a.Status,
                    FeaturedImageUrl = a.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => new ArticleImageDto
                        {
                            Id = i.Id,
                            Url = i.Url,
                            IsPrimary = i.IsPrimary,
                            DisplayOrder = i.DisplayOrder
                        })
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<AllArticlesDto>(
                items.AsReadOnly(),
                totalCount,
                request.NormalizedPageIndex,
                request.NormalizedPageSize);

            return Result<PagedResult<AllArticlesDto>>.Success(
                pagedResult,
                "Articles retrieved successfully");
        }
    }
}