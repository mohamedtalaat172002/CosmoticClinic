using AutoMapper;
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
        : IRequestHandler<
            GetAllTreatmentsQuery,
            Result<PagedResult<AllTreatmentsDto>>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly IMapper _mapper;

        public GetAllTreatmentsQueryHandler(
            ITreatmentReadRepository treatmentReadRepository,
            IMapper mapper)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<AllTreatmentsDto>>> Handle(
            GetAllTreatmentsQuery request,
            CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            var query = _treatmentReadRepository.GetAll();

            var totalCount = await query.Where(a => !a.IsDeleted).CountAsync(cancellationToken);

            query = _treatmentReadRepository
                .GetAllBySearchCriteria(query, request);

            if (query is null)
            {
                return Result<PagedResult<AllTreatmentsDto>>
                    .Failure("Treatments not found", ErrorType.NotFound);
            }

            var treatments = await query
                .OrderBy(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<AllTreatmentsDto>>(treatments);

            var pagedResult = new PagedResult<AllTreatmentsDto>(
                dtos.AsReadOnly(),
                totalCount,
                pageIndex,
                pageSize
            );

            return Result<PagedResult<AllTreatmentsDto>>
                .Success(pagedResult, "Hello from the other message hahahhahahha");
        }
    }
}