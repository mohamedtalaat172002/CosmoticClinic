using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using Clinic.Domain.Entities.artical;
using MediatR;

namespace Clinic.Application.Features.Articles.Commands.Handlers
{
    public sealed class UpdateArticleCommandHandler
        : IRequestHandler<UpdateArticleCommand, Result<UpdateCreateSingleArticleDto>>
    {
        private const string ImageFolder = "clinic/articles";

        private readonly IArticleWriteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        private readonly IImageStorage _imageStorage;

        public UpdateArticleCommandHandler(
            IArticleWriteRepository repository,
            IUnitOfWork unitOfWork,
            ISlugService slugService,
            IImageStorage imageStorage)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _slugService = slugService;
            _imageStorage = imageStorage;
        }

        public async Task<Result<UpdateCreateSingleArticleDto>> Handle(
            UpdateArticleCommand request,
            CancellationToken cancellationToken)
        {
            // Must be a TRACKED entity with Images included (no AsNoTracking).
            var article = await _repository.GetWithImagesForUpdateAsync(
                request.Id,
                cancellationToken);

            if (article is null)
            {
                return Result<UpdateCreateSingleArticleDto>.Failure(
                    "Article not found.",
                    ErrorType.NotFound);
            }

            ApplyChanges(article, request);

            var uploadedPublicIds = new List<string>();
            var replacedPublicIds = new List<string>();

            try
            {
                if (request.Images is { Count: > 0 })
                {
                    replacedPublicIds = await ReplaceImagesAsync(
                        article,
                        request,
                        uploadedPublicIds,
                        cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                // Nothing was committed: remove the assets uploaded in this request.
                await _imageStorage.DeleteAssetsQuietlyAsync(uploadedPublicIds);
                throw;
            }

            // Committed: the old assets are no longer referenced, safe to remove.
            await _imageStorage.DeleteAssetsQuietlyAsync(replacedPublicIds);

            return Result<UpdateCreateSingleArticleDto>.Success(
                MapToDto(article),
                "Article updated successfully.");
        }

        private void ApplyChanges(Article article, UpdateArticleCommand request)
        {
            article.Title = request.Title;
            article.Summary = request.Summary;
            article.Content = request.Content;
            article.Status = request.Status;
            article.TreatmentId = request.TreatmentId;
            article.Slug = _slugService.GenerateSlug(request.Title, article.Id);
            article.UpdatedAt = DateTime.UtcNow; // previously set inside repository.Update()
        }

        /// <summary>
        /// Uploads the new images and swaps them into the tracked aggregate.
        /// Returns the PublicIds of the replaced images; the caller deletes them after commit.
        /// </summary>
        private async Task<List<string>> ReplaceImagesAsync(
            Article article,
            UpdateArticleCommand request,
            List<string> uploadedPublicIds,
            CancellationToken cancellationToken)
        {
            var oldImages = article.Images.ToList();
            var displayOrder = 0;

            foreach (var file in request.Images!)
            {
                var uploaded = await _imageStorage.UploadAsync(
                    file.FileStream,
                    file.FileName,
                    ImageFolder,
                    cancellationToken);

                uploadedPublicIds.Add(uploaded.PublicId);

                article.Images.Add(new ArticleImage(
                    article.Id,
                    uploaded.Url,
                    uploaded.PublicId,
                    displayOrder,
                    isPrimary: displayOrder == 0));

                displayOrder++;
            }

            foreach (var oldImage in oldImages)
                article.Images.Remove(oldImage);

            return oldImages.Select(x => x.PublicId).ToList();
        }

        private static UpdateCreateSingleArticleDto MapToDto(Article article) => new()
        {
            Id = article.Id,
            Slug = article.Slug,
            Title = article.Title,
            Summary = article.Summary,
            Content = article.Content,
            Status = article.Status.ToString(),
            Images = article.Images
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ArticleImageDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    IsPrimary = x.IsPrimary,
                    DisplayOrder = x.DisplayOrder
                })
                .ToList()
        };
    }
}