using AutoMapper;
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, Result<PagedResult<AllArticlesDto>>>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IMapper _mapper;
        public GetAllArticlesQueryHandler(IArticleReadRepository articleReadRepository, IMapper mapper)
        {
            _articleReadRepository = articleReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<AllArticlesDto>>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize ?? 10;

            var query = _articleReadRepository.GetAll();
            var totalCount = await query.Where(a => !a.IsDeleted).CountAsync(cancellationToken);



            query = _articleReadRepository.GetAllBySearchCriteria(query, request);
            if (query is null)
                return Result<PagedResult<AllArticlesDto>>.Failure("Articles not found", ErrorType.NotFound);
            var articles = await query.OrderBy(p => p.CreatedAt)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<AllArticlesDto>>(articles);


            var pagedResult = new PagedResult<AllArticlesDto>(
                dtos.AsReadOnly(),
                totalCount,
                pageIndex,
                pageSize
            );

            return Result<PagedResult<AllArticlesDto>>.Success(pagedResult, "Articles retrieved successfully");
        }
    }
}
