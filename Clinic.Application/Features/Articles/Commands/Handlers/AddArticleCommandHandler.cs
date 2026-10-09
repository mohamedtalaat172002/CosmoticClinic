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
    public sealed class AddArticleCommandHandler
        : IRequestHandler<AddArticleCommand, Result<UpdateCreateSingleArticleDto>>
    {
        private const string ImageFolder = "clinic/articles";

        private readonly IArticleWriteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        private readonly IImageStorage _imageStorage;

        public AddArticleCommandHandler(
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
            AddArticleCommand request,
            CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Title = request.Title,
                Summary = request.Summary,
                Content = request.Content,
                Status = request.Status,
                TreatmentId = request.TreatmentId
            };

            article.Slug = _slugService.GenerateSlug(request.Title, article.Id);

            var uploadedPublicIds = new List<string>();

            try
            {
                await UploadImagesAsync(article, request, uploadedPublicIds, cancellationToken);

                await _repository.AddAsync(article);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                // Nothing was committed: remove the assets uploaded in this request.
                await _imageStorage.DeleteAssetsQuietlyAsync(uploadedPublicIds);
                throw;
            }

            return Result<UpdateCreateSingleArticleDto>.Success(
                MapToDto(article),
                "Article added successfully.");
        }

        private async Task UploadImagesAsync(
            Article article,
            AddArticleCommand request,
            List<string> uploadedPublicIds,
            CancellationToken cancellationToken)
        {
            if (request.Images is null)
                return;

            var displayOrder = 0;

            foreach (var file in request.Images)
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