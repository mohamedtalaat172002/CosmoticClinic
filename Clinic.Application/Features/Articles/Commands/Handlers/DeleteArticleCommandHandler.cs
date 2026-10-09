using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Services;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Handlers
{
    public sealed class DeleteArticleCommandHandler
        : IRequestHandler<DeleteArticleCommand, Result<bool>>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorage _imageStorage;

        public DeleteArticleCommandHandler(
            IArticleReadRepository articleReadRepository,
            IArticleWriteRepository articleWriteRepository,
            IUnitOfWork unitOfWork,
            IImageStorage imageStorage)
        {
            _articleReadRepository = articleReadRepository;
            _articleWriteRepository = articleWriteRepository;
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
        }

        public async Task<Result<bool>> Handle(
            DeleteArticleCommand request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository.GetById(request.Id);

            if (article is null)
                return Result<bool>.Failure("Article not found.", ErrorType.NotFound);

            foreach (var image in article.Images)
                await _imageStorage.DeleteAsync(image.PublicId, cancellationToken);

            _articleWriteRepository.Remove(article);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, "Article deleted successfully.");
        }
    }
}
