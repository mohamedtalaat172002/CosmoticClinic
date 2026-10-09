
using Clinic.Application.Interfaces.Services.ImgStorage;

namespace Clinic.Application.Interfaces.Services
{
    public interface IImageStorage
    {
        Task<ImgUploadResult> UploadAsync(
            Stream fileStream,
            string fileName,
            string folder,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string publicId,
            CancellationToken cancellationToken = default);

        Task<ImgUploadResult> ReplaceAsync(
            Stream fileStream,
            string fileName,
            string folder,
            string oldPublicId,
            CancellationToken cancellationToken = default);
        Task DeleteAssetsQuietlyAsync(IEnumerable<string> publicIds);


    }
}

