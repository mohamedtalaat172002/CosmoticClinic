using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var treatment = await _treatmentReadRepository
                .GetAll()
                .Include(t => t.Images)
                .FirstOrDefaultAsync(
                    t => t.Slug == request.Slug && !t.IsDeleted,
                    cancellationToken);

            if (treatment is null)
            {
                return Result<SingleTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            var articles = await GetArticlesPageAsync(treatment.Id, request, cancellationToken);

            var result = new SingleTreatmentDto
            {
                Id = treatment.Id,
                Slug = treatment.Slug,
                Name = treatment.Name,
                Description = treatment.Description,
                Images = treatment.Images
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new TreatmentImageDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        IsPrimary = x.IsPrimary,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToList(),
                Articles = articles
            };

            return Result<SingleTreatmentDto>.Success(result);
        }

        private async Task<PagedResult<AllArticlesDto>> GetArticlesPageAsync(
            Guid treatmentId,
            GetTreatmentBySlugQuery request,
            CancellationToken cancellationToken)
        {

            var baseQuery = _articleReadRepository
                .GetAll()
                .Include(a => a.Images.Where(i => i.IsPrimary))
                .Where(a => a.TreatmentId == treatmentId && !a.IsDeleted);

            var filtered = _articleReadRepository.ApplyFilters(baseQuery, request);

            var totalCount = await filtered.CountAsync(cancellationToken);

            var ordered = filtered
                .OrderByDescending(a => a.CreatedAt)
                .ThenBy(a => a.Id);

            var items = await _articleReadRepository
                .ApplyPaging(ordered, request)
                .ProjectTo<AllArticlesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<AllArticlesDto>(
                items,
                totalCount,
                request.NormalizedPageIndex,
                request.NormalizedPageSize);
        }
    }
}