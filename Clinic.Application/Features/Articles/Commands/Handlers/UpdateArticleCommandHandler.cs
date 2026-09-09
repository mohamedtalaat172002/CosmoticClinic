using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Services;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Handlers
{
    public sealed class UpdateArticleCommandHandler
     : IRequestHandler<UpdateArticleCommand, Result<SingleArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        public UpdateArticleCommandHandler(
            IMapper mapper,
            IArticleReadRepository articleReadRepository,
            IArticleWriteRepository articleWriteRepository,
            IUnitOfWork unitOfWork,
            ISlugService slugService)
        {
            _mapper = mapper;
            _articleReadRepository = articleReadRepository;
            _articleWriteRepository = articleWriteRepository;
            _unitOfWork = unitOfWork;
            _slugService = slugService;
        }

        public async Task<Result<SingleArticleDto>> Handle(
            UpdateArticleCommand request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository.GetById(request.Id);

            if (article is null)
            {
                return Result<SingleArticleDto>.Failure(
                    "Article not found.",
                    ErrorType.NotFound);
            }


            _mapper.Map(request, article);
            article.Slug = _slugService.GenerateSlug(request.Title, article.Id);
            _articleWriteRepository.Update(article);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var articleDto = _mapper.Map<SingleArticleDto>(article);

            return Result<SingleArticleDto>.Success(
                articleDto,
                "Article updated successfully.");
        }
    }
}
