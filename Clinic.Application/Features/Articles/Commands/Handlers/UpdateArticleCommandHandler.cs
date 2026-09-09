using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
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

        public UpdateArticleCommandHandler(
            IMapper mapper,
            IArticleReadRepository articleReadRepository,
            IArticleWriteRepository articleWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _articleReadRepository = articleReadRepository;
            _articleWriteRepository = articleWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SingleArticleDto>> Handle(
            UpdateArticleCommand request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository.GetById(
                request.Id);

            if (article is null)
            {
                return Result<SingleArticleDto>.Failure(
                    "Article not found.",
                    ErrorType.NotFound);
            }

            _mapper.Map(request, article);

            _articleWriteRepository.Update(article);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var articleDto = _mapper.Map<SingleArticleDto>(article);

            return Result<SingleArticleDto>.Success(
                articleDto,
                "Article updated successfully.");
        }
    }
}
