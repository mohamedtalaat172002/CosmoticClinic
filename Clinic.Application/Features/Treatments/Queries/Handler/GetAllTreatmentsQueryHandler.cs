using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Treatments.Queries.Handler
{
    internal sealed class GetAllTreatmentsQueryHandler
        : IRequestHandler<GetAllTreatmentsQuery, Result<PagedResult<AllTreatmentsDto>>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private const int ExcerptLength = 220;

        public GetAllTreatmentsQueryHandler(ITreatmentReadRepository treatmentReadRepository)
        {
            _treatmentReadRepository = treatmentReadRepository;
        }

        public async Task<Result<PagedResult<AllTreatmentsDto>>> Handle(
            GetAllTreatmentsQuery request,
            CancellationToken cancellationToken)
        {
            // Same filtered query feeds both the count and the page => they always match.
            var filtered = _treatmentReadRepository.ApplyFilters(
                _treatmentReadRepository.GetAll(),
                request);

            var totalCount = await filtered.CountAsync(cancellationToken);

            // Order BEFORE paging; ThenBy(Id) keeps the order stable between pages.
            var ordered = filtered
                .OrderBy(t => t.CreatedAt)
                .ThenBy(t => t.Id);

            // Project only the columns needed; the primary image URL is a subquery,
            // so no Include and no other images are loaded.
            var items = await _treatmentReadRepository
                .ApplyPaging(ordered, request)
                .Select(t => new AllTreatmentsDto
                {
                    Id = t.Id,
                    Slug = t.Slug,
                    Name = t.Name,
                    Description = t.Description.Length > ExcerptLength
                        ? t.Description.Substring(0, ExcerptLength)
                        : t.Description,

                    ImageUrl = t.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.Url)
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<AllTreatmentsDto>(
                items.AsReadOnly(),
                totalCount,
                request.NormalizedPageIndex,
                request.NormalizedPageSize);

            return Result<PagedResult<AllTreatmentsDto>>.Success(pagedResult);
        }
    }
}