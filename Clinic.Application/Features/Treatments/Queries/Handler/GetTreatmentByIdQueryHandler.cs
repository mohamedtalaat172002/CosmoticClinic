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
    internal sealed class GetTreatmentByIdQueryHandler
    : IRequestHandler<GetTreatmentByIdQuery, Result<SingleTreatmentDto>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly IMapper _mapper;
        private readonly IArticleReadRepository _articleReadRepository;
        public GetTreatmentByIdQueryHandler(
            ITreatmentReadRepository treatmentReadRepository,
            IMapper mapper,
            IArticleReadRepository articleReadRepository)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _mapper = mapper;
            _articleReadRepository = articleReadRepository;
        }

        public async Task<Result<SingleTreatmentDto>> Handle(
            GetTreatmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            var query = _articleReadRepository.GetAll().Where(a => a.TreatmentId == request.Id);

            var totalArticleCount = await query.CountAsync(cancellationToken);

            query = _articleReadRepository.GetAllBySearchCriteria(query, request);

            var articles = await query.OrderBy(p => p.CreatedAt)
                .ToListAsync(cancellationToken);

            var pagedArticles = new PagedResult<AllArticlesDto>(
                _mapper.Map<List<AllArticlesDto>>(articles),
                totalArticleCount,
                pageIndex,
                pageSize
            );

            var treatment = await _treatmentReadRepository.GetById(request.Id);

            var result = new SingleTreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                Description = treatment.Description,
                ImageUrl = treatment.ImageUrl,
                Articles = pagedArticles
            };
            return Result<SingleTreatmentDto>.Success(result);

        }
    }
}
