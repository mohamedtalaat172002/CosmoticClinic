using AutoMapper;
using BackEnd.Domain.Entities;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Products.Dto;
using Clinic.Application.Interfaces.Services;
using MediatR;

namespace Clinic.Application.Features.Products.Command
{
    public record CreateProductCommand(
         string Name,
         string? Description,
         decimal Price
     ) : IRequest<Result<ReturnProductDto>>;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ReturnProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateProductHandler(
            IUnitOfWork unitOfWork,
            IIdentityService identityService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<Result<ReturnProductDto>> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var userId = _identityService.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Result<ReturnProductDto>.Failure("User not authenticated", ErrorType.Unauthorized);

            var product = new Product
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Price = request.Price,
                UserId = userId
            };

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);

            var dto = _mapper.Map<ReturnProductDto>(product);

            return Result<ReturnProductDto>.Success(dto, "Product created successfully");
        }
    }
}
