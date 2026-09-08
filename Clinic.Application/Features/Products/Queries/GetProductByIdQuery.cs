using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Products.Dto;
using MediatR;

namespace Clinic.Application.Features.Products.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<Result<ReturnProductDto>>;

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ReturnProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ReturnProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            var product = await _unitOfWork.ProductReadRepository.GetById(request.Id);

            if (product is null || product.IsDeleted)
                return Result<ReturnProductDto>.Failure("Product not found", ErrorType.NotFound);

            var dto = new ReturnProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                UserId = product.UserId,
                CreatedOn = product.CreatedAt,
            };

            return Result<ReturnProductDto>.Success(dto);
        }
    }
}
