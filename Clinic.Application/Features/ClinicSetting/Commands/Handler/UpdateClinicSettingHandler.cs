//1- Get the clinicSettings from DB
//2-Applychanges: update everything except images
//3-ReplaceImagesAsync:
// 3.1-upload new images
// 3.2 -add new images to settings.Images [DB], also get the publicIds of old images to delete later
// 3.3 -remove old images from settings.Images [DB]
//4-save changes to DB
// if any exception occurs during the process, delete the newly uploaded images from cloudinary using their publicIds
//5-DeleteAssetsQuietlyAsync: delete old images from cloudinary using their publicIds

using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using Clinic.Domain.Entities.clinicSettings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clinic.Application.Features.ClinicSetting.Commands.Handler
{
    public class UpdateClinicSettingHandler
        : IRequestHandler<UpdateClinicSettingModel, Result<UpdateCreateClinicSettingDto>>
    {
        private const string ImageFolder = "clinic/settings";

        private static readonly Guid ClinicSettingsId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private readonly IClinicSettingWriteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<UpdateClinicSettingHandler> _logger;

        public UpdateClinicSettingHandler(
            IClinicSettingWriteRepository repository,
            IUnitOfWork unitOfWork,
            IImageStorage imageStorage,
            ILogger<UpdateClinicSettingHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
            _logger = logger;
        }

        public async Task<Result<UpdateCreateClinicSettingDto>> Handle(
            UpdateClinicSettingModel request,
            CancellationToken cancellationToken)
        {
            var settings = await _repository.GetWithImagesForUpdateAsync(
                ClinicSettingsId,
                cancellationToken);

            if (settings is null)
            {
                return Result<UpdateCreateClinicSettingDto>.Failure(
                    "Clinic settings not found.",
                    ErrorType.NotFound);
            }

            ApplyChanges(settings, request);

            var uploadedPublicIds = new List<string>();
            var replacedPublicIds = new List<string>();

            try
            {
                if (request.Images is { Count: > 0 })
                {
                    replacedPublicIds = await ReplaceImagesAsync(
                        settings,
                        request,
                        uploadedPublicIds,
                        cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await DeleteAssetsQuietlyAsync(uploadedPublicIds);
                throw;
            }

            await DeleteAssetsQuietlyAsync(replacedPublicIds);

            return Result<UpdateCreateClinicSettingDto>.Success(
                MapToDto(settings),
                "Clinic settings updated successfully.");
        }

        private static void ApplyChanges(ClinicSettings settings, UpdateClinicSettingModel request)
        {
            settings.ClinicName = request.ClinicName;
            settings.DoctorName = request.DoctorName;
            settings.Specialization = request.Specialization;
            settings.Description = request.Description;
            settings.WhatsAppNumber = request.WhatsAppNumber;
            settings.Email = request.Email;
            settings.Address = request.Address;
            settings.FacebookUrl = request.FacebookUrl;
            settings.InstagramUrl = request.InstagramUrl;
            settings.UpdatedAt = DateTime.UtcNow;
        }

        private async Task<List<string>> ReplaceImagesAsync(
            ClinicSettings settings,
            UpdateClinicSettingModel request,
            List<string> uploadedPublicIds,
            CancellationToken cancellationToken)
        {
            var oldImages = settings.Images.ToList();
            var displayOrder = 0;

            foreach (var file in request.Images!)
            {
                var uploaded = await _imageStorage.UploadAsync(
                    file.FileStream,
                    file.FileName,
                    ImageFolder,
                    cancellationToken);

                uploadedPublicIds.Add(uploaded.PublicId);

                settings.Images.Add(new ClinicSettingImage(
                    settings.Id,
                    uploaded.Url,
                    uploaded.PublicId,
                    displayOrder,
                    isPrimary: displayOrder == 0));

                displayOrder++;
            }

            foreach (var oldImage in oldImages)
                settings.Images.Remove(oldImage);

            return oldImages.Select(x => x.PublicId).ToList();
        }

        private async Task DeleteAssetsQuietlyAsync(IEnumerable<string> publicIds)
        {
            foreach (var publicId in publicIds)
            {
                try
                {
                    await _imageStorage.DeleteAsync(publicId, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete Cloudinary asset {PublicId}", publicId);
                }
            }
        }

        private static UpdateCreateClinicSettingDto MapToDto(ClinicSettings settings) => new()
        {
            Id = settings.Id,
            ClinicName = settings.ClinicName,
            DoctorName = settings.DoctorName,
            Specialization = settings.Specialization,
            Description = settings.Description,
            WhatsAppNumber = settings.WhatsAppNumber,
            Email = settings.Email,
            Address = settings.Address,
            FacebookUrl = settings.FacebookUrl,
            InstagramUrl = settings.InstagramUrl,
            Images = settings.Images
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ClinicSettingImageDto
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