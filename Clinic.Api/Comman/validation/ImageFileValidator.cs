namespace Clinic.Api.Common.Validation
{
    public static class ImageFileValidator
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private static readonly Dictionary<string, string[]> AllowedContentTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["image/jpeg"] = [".jpg", ".jpeg"],
                ["image/png"] = [".png"],
                ["image/webp"] = [".webp"]
            };

        public static async Task<string?> ValidateAsync(IFormFile file)
        {
            if (file.Length == 0)
                return "The image file is empty.";

            if (file.Length > MaxFileSize)
                return "The image size must not exceed 5 MB.";

            if (!AllowedContentTypes.TryGetValue(
                    file.ContentType,
                    out var allowedExtensions))
            {
                return "Only JPG, JPEG, PNG, and WEBP images are allowed.";
            }

            var extension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(
                    extension,
                    StringComparer.OrdinalIgnoreCase))
            {
                return "The file extension does not match the allowed image type.";
            }

            await using var stream = file.OpenReadStream();

            var buffer = new byte[12];

            var bytesRead = await stream.ReadAsync(buffer);

            if (!IsValidImageSignature(
                    buffer,
                    bytesRead,
                    file.ContentType))
            {
                return "The uploaded file is not a valid image.";
            }

            return null;
        }

        private static bool IsValidImageSignature(
            byte[] buffer,
            int bytesRead,
            string contentType)
        {
            if (contentType.Equals(
                    "image/jpeg",
                    StringComparison.OrdinalIgnoreCase))
            {
                return bytesRead >= 3
                    && buffer[0] == 0xFF
                    && buffer[1] == 0xD8
                    && buffer[2] == 0xFF;
            }

            if (contentType.Equals(
                    "image/png",
                    StringComparison.OrdinalIgnoreCase))
            {
                return bytesRead >= 8
                    && buffer[0] == 0x89
                    && buffer[1] == 0x50
                    && buffer[2] == 0x4E
                    && buffer[3] == 0x47
                    && buffer[4] == 0x0D
                    && buffer[5] == 0x0A
                    && buffer[6] == 0x1A
                    && buffer[7] == 0x0A;
            }

            if (contentType.Equals(
                    "image/webp",
                    StringComparison.OrdinalIgnoreCase))
            {
                return bytesRead >= 12
                    && buffer[0] == 0x52
                    && buffer[1] == 0x49
                    && buffer[2] == 0x46
                    && buffer[3] == 0x46
                    && buffer[8] == 0x57
                    && buffer[9] == 0x45
                    && buffer[10] == 0x42
                    && buffer[11] == 0x50;
            }

            return false;
        }
    }
}