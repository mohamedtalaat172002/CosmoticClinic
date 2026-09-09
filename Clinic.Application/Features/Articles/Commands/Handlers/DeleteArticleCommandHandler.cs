using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Handlers
{
    public sealed class DeleteArticleCommandHandler
    : IRequestHandler<DeleteArticleCommand, Result<bool>>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteArticleCommandHandler(
            IArticleReadRepository articleReadRepository,
            IArticleWriteRepository articleWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _articleReadRepository = articleReadRepository;
            _articleWriteRepository = articleWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(
            DeleteArticleCommand request,
            CancellationToken cancellationToken)
        {
            var article = await _articleReadRepository.GetById(request.Id);

            if (article is null)
            {
                return Result<bool>.Failure(
                    "Article not found.",
                    ErrorType.NotFound);
            }

            _articleWriteRepository.Remove(article);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(
                true,
                "Article deleted successfully.");
        }
    }
}
