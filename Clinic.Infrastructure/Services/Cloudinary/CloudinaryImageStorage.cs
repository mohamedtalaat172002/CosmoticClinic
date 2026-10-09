using Clinic.Application.Interfaces.Services;
using Clinic.Application.Interfaces.Services.ImgStorage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;

namespace Clinic.Infrastructure.Services.Cloudinary
{
    public class CloudinaryImageStorage : IImageStorage
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryImageStorage> _logger;
        public CloudinaryImageStorage(
            CloudinaryDotNet.Cloudinary cloudinary, ILogger<CloudinaryImageStorage> logger)
        {
            _cloudinary = cloudinary;
            _logger = logger;
        }

        public async Task<ImgUploadResult> UploadAsync(
            Stream fileStream,
            string fileName,
            string folder,
            CancellationToken cancellationToken = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(
                    fileName,
                    fileStream),

                Folder = folder
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                throw new InvalidOperationException(
                    result.Error.Message);
            }

            return new ImgUploadResult
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }

        public async Task DeleteAsync(
            string publicId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                throw new InvalidOperationException(
                    result.Error.Message);
            }
        }

        public async Task<ImgUploadResult> ReplaceAsync(
            Stream fileStream,
            string fileName,
            string folder,
            string oldPublicId,
            CancellationToken cancellationToken = default)
        {
            var newImage = await UploadAsync(
                fileStream,
                fileName,
                folder,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(oldPublicId))
            {
                await DeleteAsync(
                    oldPublicId,
                    cancellationToken);
            }

            return newImage;
        }


        public async Task DeleteAssetsQuietlyAsync(IEnumerable<string> publicIds)
        {
            foreach (var publicId in publicIds)
            {
                try
                {
                    await DeleteAsync(publicId, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete Cloudinary asset {PublicId}", publicId);
                }
            }
        }




    }
}
