namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for validating and converting base64 images .
    /// </summary>
    public class ImageValidatorService(
        IHeaderService headerService,
        BaseLogger<ImageValidatorService> logger) : IImageValidatorService
    {
        private readonly IHeaderService _headerService = headerService ?? throw new ArgumentException(nameof(headerService));
        private readonly BaseLogger<ImageValidatorService> _logger = logger ?? throw new ArgumentException(nameof(logger));
        private const int MaxImageSizeBytes = 15 * 1024 * 1024;

        /// <summary>
        /// Converts a base64 image string to a MemoryStream, validates its size and mime type, and generates a file name.
        /// Throws <see cref="InvalidFieldException"/> if the image is too large or has an invalid mime type.
        /// </summary>
        /// <param name="identifier">Identifier for the delivery partner.</param>
        /// <param name="base64Image">Base64 string of the image.</param>
        /// <returns>Tuple containing the image stream, file name, and content type.</returns>
        public (MemoryStream stream, string fileName, string contentType) ConvertBase64ImageToStream(string identifier, string base64Image)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(ConvertBase64ImageToStream);

            var imageBytes = Convert.FromBase64String(base64Image);

            if (imageBytes.Length > MaxImageSizeBytes)
            {
                _logger.LogWarning(correlationId, userId, methodName, "Image exceeding 15MB.");
                throw new InvalidFieldException(nameof(base64Image));
            }

            var mimeType = GetImageMimeType(imageBytes);
            if (mimeType != "image/png" && mimeType != "image/bmp")
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Invalid MimeType: {mimeType}");
                throw new InvalidFieldException(nameof(base64Image));
            }

            var fileName = $"{identifier}_driverlicense_{DateTime.Now:ddMMyyyyHH:mm}";
            return (new MemoryStream(imageBytes), fileName, mimeType);
        }

        /// <summary>
        /// Determines the mime type of an image based on its byte signature.
        /// </summary>
        /// <param name="bytes">Byte array of the image.</param>
        /// <returns>Mime type string ("image/png", "image/bmp", or "unknown").</returns>
        private static string GetImageMimeType(byte[] bytes)
        {
            return bytes.Length switch
            {
                // PNG: 89 50 4E 47 0D 0A 1A 0A
                > 8 when bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 &&
                         bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A => "image/png",
                // BMP: 42 4D
                > 2 when bytes[0] == 0x42 && bytes[1] == 0x4D => "image/bmp",
                _ => "unknown"
            };
        }
    }
}
