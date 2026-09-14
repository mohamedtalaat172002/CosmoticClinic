using AutoMapper;
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Queries.Models;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Consultations.Queries.Handlers
{
    public class GetAllConsultationQueryHandler : IRequestHandler<GetAllConsultationQuery, Result<PagedResult<AllConsultationsDto>>>
    {

        private readonly IMapper _mapper;
        private readonly IConsultationReadRepository _consultationReadRepository;

        public GetAllConsultationQueryHandler(IMapper mapper, IConsultationReadRepository consultationReadRepository)
        {
            _mapper = mapper;
            _consultationReadRepository = consultationReadRepository;
        }

        public async Task<Result<PagedResult<AllConsultationsDto>>> Handle(GetAllConsultationQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            var query = _consultationReadRepository.GetAll();

            var totalCount = await query.CountAsync(cancellationToken);

            query = _consultationReadRepository.GetAllBySearchCriteria(query, request);
            if (query is null)
                return Result<PagedResult<AllConsultationsDto>>.Failure("No Consultations found", ErrorType.NotFound);

            var consultations = await query.OrderBy(p => p.CreatedAt)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<AllConsultationsDto>>(consultations);


            var pagedResult = new PagedResult<AllConsultationsDto>(
                dtos.AsReadOnly(),
                totalCount,
                pageIndex,
                pageSize
            );

            return Result<PagedResult<AllConsultationsDto>>.Success(pagedResult);


        }
    }
}
