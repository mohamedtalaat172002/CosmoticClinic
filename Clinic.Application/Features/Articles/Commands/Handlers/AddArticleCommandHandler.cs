using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities.artical;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Handlers
{
    public sealed class AddArticleCommandHandler : IRequestHandler<AddArticleCommand, Result<SingleArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IArticleWriteRepository _articleWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        public AddArticleCommandHandler(IMapper mapper, IArticleWriteRepository articleWriteRepository, IUnitOfWork unitOfWork, ISlugService slugService)
        {
            _mapper = mapper;
            _articleWriteRepository = articleWriteRepository;
            _unitOfWork = unitOfWork;
            _slugService = slugService;
        }

        public async Task<Result<SingleArticleDto>> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(request);
            article.Slug = _slugService.GenerateSlug(request.Title, article.Id);
            await _articleWriteRepository.AddAsync(article);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var articleDto = _mapper.Map<SingleArticleDto>(article);

            return Result<SingleArticleDto>.Success(
                articleDto,
                "Article added successfully.");
        }
    }
}
