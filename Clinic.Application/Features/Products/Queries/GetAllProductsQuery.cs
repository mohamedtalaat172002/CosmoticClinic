using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Products.Dto;
using Clinic.Application.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.Products.Queries;

public class GetAllProductsQuery()
    : ProductSearchCriteria, IRequest<Result<PagedResult<ReturnProductDto>>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<PagedResult<ReturnProductDto>>>
{
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductsHandler(
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<ReturnProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken ct)
    {

        var userId = _identityService.GetUserId();

        var pageIndex = request.PageIndex ?? 1;
        var pageSize = request.PageSize ?? 10;

        var query = _unitOfWork.ProductReadRepository.GetAll();
        query = _unitOfWork.ProductReadRepository.GetAllBySearchCriteria(query, request);

        var total = await query.CountAsync(ct);
        var products = await query.OrderBy(p => p.Name)
            .ToListAsync(ct);

        var dtos = products.Select(p => new ReturnProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            UserId = p.UserId,
            CreatedOn = p.CreatedAt,

        }).ToList();

        var pagedResult = new PagedResult<ReturnProductDto>(
            dtos.AsReadOnly(),
            total,
            pageIndex,
            pageSize
        );

        return Result<PagedResult<ReturnProductDto>>.Success(pagedResult);
    }
}