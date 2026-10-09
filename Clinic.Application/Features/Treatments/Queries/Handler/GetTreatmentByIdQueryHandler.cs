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
    internal sealed class GetTreatmentByIdQueryHandler
        : IRequestHandler<GetTreatmentByIdQuery, Result<AdminTreatmentDto>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly IArticleReadRepository _articleReadRepository;

        public GetTreatmentByIdQueryHandler(
            ITreatmentReadRepository treatmentReadRepository,
            IArticleReadRepository articleReadRepository)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _articleReadRepository = articleReadRepository;
        }

        public async Task<Result<AdminTreatmentDto>> Handle(
            GetTreatmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var treatment = await _treatmentReadRepository
                .GetAll()
                .Include(t => t.Images)
                .FirstOrDefaultAsync(
                    t => t.Id == request.Id,
                    cancellationToken);

            if (treatment is null)
            {
                return Result<AdminTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            var articles = await _articleReadRepository
                .GetAll()
                .Where(a =>
                    a.TreatmentId == treatment.Id &&
                    !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ThenBy(a => a.Id)
                .Select(a => new AllArticlesDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Status = a.Status,
                    Slug = a.Slug,
                    Summary = a.Summary,

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

            var result = new AdminTreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                Slug = treatment.Slug,
                Description = treatment.Description,
                IsDeleted = treatment.IsDeleted,
                CreatedAt = treatment.CreatedAt,
                UpdatedAt = treatment.UpdatedAt,

                Images = treatment.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new TreatmentImageDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        IsPrimary = i.IsPrimary,
                        DisplayOrder = i.DisplayOrder
                    })
                    .ToList(),

                Articles = new PagedResult<AllArticlesDto>(
                    articles,
                    articles.Count,
                    1,
                    articles.Count)
            };

            return Result<AdminTreatmentDto>.Success(result);
        }
    }
}
