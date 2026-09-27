using AutoMapper;
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Treatments.Queries.Handler
{
    internal sealed class GetTreatmentBySlugQueryHandler
        : IRequestHandler<GetTreatmentBySlugQuery, Result<SingleTreatmentDto>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IMapper _mapper;

        public GetTreatmentBySlugQueryHandler(
            ITreatmentReadRepository treatmentReadRepository,
            IArticleReadRepository articleReadRepository,
            IMapper mapper)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _articleReadRepository = articleReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<SingleTreatmentDto>> Handle(
            GetTreatmentBySlugQuery request,
            CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            var treatment = await _treatmentReadRepository
                .GetBySlugAsync(request.Slug);

            if (treatment is null || treatment.IsDeleted)
            {
                return Result<SingleTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            // Scope to this treatment's articles, excluding deleted ones
            var baseQuery = _articleReadRepository
                .GetAll()
                .Where(a => a.TreatmentId == treatment.Id && !a.IsDeleted);

            // Apply search filter before counting so total matches paged data
            var search = request.Search?.Trim();
            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(a =>
                    Microsoft.EntityFrameworkCore.EF.Functions.Like(a.Title, $"%{search}%") ||
                    Microsoft.EntityFrameworkCore.EF.Functions.Like(a.Summary, $"%{search}%") ||
                    Microsoft.EntityFrameworkCore.EF.Functions.Like(a.Content, $"%{search}%"));
            }

            var totalArticleCount = await baseQuery.CountAsync(cancellationToken);

            var articles = await baseQuery
                .OrderBy(p => p.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var pagedArticles = new PagedResult<AllArticlesDto>(
                _mapper.Map<List<AllArticlesDto>>(articles),
                totalArticleCount,
                pageIndex,
                pageSize
            );

            var result = new SingleTreatmentDto
            {
                Id = treatment.Id,
                Slug = treatment.Slug,
                Name = treatment.Name,
                Description = treatment.Description,
                ImageUrl = treatment.ImageUrl,
                Articles = pagedArticles
            };

            return Result<SingleTreatmentDto>.Success(result);
        }
    }
}