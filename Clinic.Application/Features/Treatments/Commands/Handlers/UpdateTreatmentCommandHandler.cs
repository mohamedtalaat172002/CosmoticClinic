using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using MediatR;
using Treatment = Clinic.Domain.Entities.treatment.Treatment;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class UpdateTreatmentCommandHandler
        : IRequestHandler<UpdateTreatmentCommand, Result<UpdateCreateSingleTreatmentDto>>
    {
        private const string ImageFolder = "clinic/treatments";

        private readonly ITreatmentWriteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        private readonly IImageStorage _imageStorage;

        public UpdateTreatmentCommandHandler(
            ITreatmentWriteRepository repository,
            IUnitOfWork unitOfWork,
            ISlugService slugService,
            IImageStorage imageStorage)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _slugService = slugService;
            _imageStorage = imageStorage;
        }

        public async Task<Result<UpdateCreateSingleTreatmentDto>> Handle(
            UpdateTreatmentCommand request,
            CancellationToken cancellationToken)
        {
            // Must be a TRACKED entity with Images included (no AsNoTracking).
            var treatment = await _repository.GetWithImagesForUpdateAsync(
                request.Id,
                cancellationToken);

            if (treatment is null)
            {
                return Result<UpdateCreateSingleTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            ApplyChanges(treatment, request);

            var uploadedPublicIds = new List<string>();
            var replacedPublicIds = new List<string>();

            try
            {
                if (request.Images is { Count: > 0 })
                {
                    replacedPublicIds = await ReplaceImagesAsync(
                        treatment,
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

            return Result<UpdateCreateSingleTreatmentDto>.Success(
                MapToDto(treatment),
                "Treatment updated successfully.");
        }

        private void ApplyChanges(Treatment treatment, UpdateTreatmentCommand request)
        {
            treatment.Name = request.Name;
            treatment.Description = request.Description;
            treatment.Slug = _slugService.GenerateSlug(request.Name, treatment.Id);
            treatment.UpdatedAt = DateTime.UtcNow; // previously set inside repository.Update()
        }

        /// <summary>
        /// Uploads the new images and swaps them into the tracked aggregate.
        /// Returns the PublicIds of the replaced images; the caller deletes them after commit.
        /// </summary>
        private async Task<List<string>> ReplaceImagesAsync(
            Treatment treatment,
            UpdateTreatmentCommand request,
            List<string> uploadedPublicIds,
            CancellationToken cancellationToken)
        {
            var oldImages = treatment.Images.ToList();
            var displayOrder = 0;

            foreach (var file in request.Images!)
            {
                var uploaded = await _imageStorage.UploadAsync(
                    file.FileStream,
                    file.FileName,
                    ImageFolder,
                    cancellationToken);

                uploadedPublicIds.Add(uploaded.PublicId);

                treatment.Images.Add(new TreatmentImage(
                    treatment.Id,
                    uploaded.Url,
                    uploaded.PublicId,
                    displayOrder,
                    isPrimary: displayOrder == 0));

                displayOrder++;
            }

            foreach (var oldImage in oldImages)
                treatment.Images.Remove(oldImage);

            return oldImages.Select(x => x.PublicId).ToList();
        }

        private static UpdateCreateSingleTreatmentDto MapToDto(Treatment treatment) => new()
        {
            Id = treatment.Id,
            Slug = treatment.Slug,
            Name = treatment.Name,
            Description = treatment.Description,
            Images = treatment.Images
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new TreatmentImageDto
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