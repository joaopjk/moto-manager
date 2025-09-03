namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the ImageValidatorService class.
    /// </summary>
    public class ImageValidatorServiceTests
    {
        private readonly Mock<IHeaderService> _headerServiceMock = new();
        private readonly Mock<ILogger<ImageValidatorService>> _loggerMock = new();

        private ImageValidatorService CreateService()
        {
            var baseLogger = new BaseLogger<ImageValidatorService>(_loggerMock.Object);
            return new ImageValidatorService(_headerServiceMock.Object, baseLogger);
        }

        [Fact]
        public void ConvertBase64ImageToStream_ValidPngImage_ReturnsStreamAndMimeType()
        {
            var service = CreateService();
            var pngBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 };
            var base64 = Convert.ToBase64String(pngBytes);
            var result = service.ConvertBase64ImageToStream("id", base64);
            Assert.NotNull(result.stream);
            Assert.Equal("image/png", result.contentType);
            Assert.StartsWith("id_driverlicense_", result.fileName);
        }

        [Fact]
        public void ConvertBase64ImageToStream_ValidBmpImage_ReturnsStreamAndMimeType()
        {
            var service = CreateService();
            var bmpBytes = new byte[] { 0x42, 0x4D, 0x00 };
            var base64 = Convert.ToBase64String(bmpBytes);
            var result = service.ConvertBase64ImageToStream("id", base64);
            Assert.NotNull(result.stream);
            Assert.Equal("image/bmp", result.contentType);
            Assert.StartsWith("id_driverlicense_", result.fileName);
        }

        [Fact]
        public void ConvertBase64ImageToStream_ImageTooLarge_ThrowsInvalidFieldException()
        {
            var service = CreateService();
            var largeBytes = new byte[15 * 1024 * 1024 + 1];
            var base64 = Convert.ToBase64String(largeBytes);
            Assert.Throws<InvalidFieldException>(() => service.ConvertBase64ImageToStream("id", base64));
        }

        [Fact]
        public void ConvertBase64ImageToStream_InvalidMimeType_ThrowsInvalidFieldException()
        {
            var service = CreateService();
            var invalidBytes = new byte[] { 0x00, 0x01, 0x02 };
            var base64 = Convert.ToBase64String(invalidBytes);
            Assert.Throws<InvalidFieldException>(() => service.ConvertBase64ImageToStream("id", base64));
        }

        [Fact]
        public void ConvertBase64ImageToStream_InvalidBase64_ThrowsFormatException()
        {
            var service = CreateService();
            Assert.Throws<FormatException>(() => service.ConvertBase64ImageToStream("id", "not_base64"));
        }
    }
}
