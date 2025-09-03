namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the DeliveryPartnerService class.
    /// </summary>
    public class DeliveryPartnerServiceTests
    {
        private readonly Mock<IDeliveryPartnerRepository> _repoMock = new();
        private readonly Mock<IMinIoStorage> _minioMock = new();
        private readonly Mock<IHeaderService> _headerServiceMock = new();
        private readonly Mock<IImageValidatorService> _imageValidatorMock = new();
        private readonly Mock<ILogger<DeliveryPartnerService>> _loggerBaseMock = new();

        private DeliveryPartnerService CreateService()
        {
            _headerServiceMock.Setup(h => h.GetRequestInfo()).Returns(("corr-id", "user-id"));
            var baseLogger = new BaseLogger<DeliveryPartnerService>(_loggerBaseMock.Object);
            return new DeliveryPartnerService(
                _repoMock.Object,
                _minioMock.Object,
                _headerServiceMock.Object,
                _imageValidatorMock.Object,
                baseLogger);
        }

        [Fact]
        public async Task RegisterDeliveryRider_InvalidDto_ReturnsFail()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto();
            var result = await service.RegisterDeliveryRider(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterDeliveryRider_CnpjExists_ReturnsFail()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto { Identifier = "id", Name = "name", Cnpj = "09077683000150", DriverLicenseNumber = "12345678901", DriverLicenseType = "A" };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(true);
            var result = await service.RegisterDeliveryRider(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterDeliveryRider_CnhExists_ReturnsFail()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto { Identifier = "id", Name = "name", Cnpj = "09077683000150", DriverLicenseNumber = "12345678901", DriverLicenseType = "A" };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(false);
            _repoMock.Setup(r => r.ExistsByCnh(dto.DriverLicenseNumber)).ReturnsAsync(true);
            var result = await service.RegisterDeliveryRider(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterDeliveryRider_RegisterSuccess_ReturnsSuccess()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto { Identifier = "id", Name = "name", Cnpj = "09077683000150", DriverLicenseNumber = "12345678901", DriverLicenseType = "A", DateOfBirth = DateTime.Now };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(false);
            _repoMock.Setup(r => r.ExistsByCnh(dto.DriverLicenseNumber)).ReturnsAsync(false);
            _repoMock.Setup(r => r.RegisterDeliveryPartner(It.IsAny<DeliveryPartnerEntity>())).ReturnsAsync(true);
            var result = await service.RegisterDeliveryRider(dto);
            Assert.True(result.Success);
            Assert.Equal(Resource.DELIVERYRIDER_UPDATED_SUCCESSFULLY, result.Message.Message);
        }

        [Fact]
        public async Task RegisterDeliveryRider_RegisterFails_ReturnsFail()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto { Identifier = "id", Name = "name", Cnpj = "09077683000150", DriverLicenseNumber = "12345678901", DriverLicenseType = "A" };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(false);
            _repoMock.Setup(r => r.ExistsByCnh(dto.DriverLicenseNumber)).ReturnsAsync(false);
            _repoMock.Setup(r => r.RegisterDeliveryPartner(It.IsAny<DeliveryPartnerEntity>())).ReturnsAsync(false);
            var result = await service.RegisterDeliveryRider(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task UploadDriverLicenseImage_DeliveryPartnerNotFound_ReturnsFail()
        {
            var service = CreateService();
            _repoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(It.IsAny<string>())).ReturnsAsync((DeliveryPartnerEntity)null);
            var result = await service.UploadDriverLicenseImage("id", "base64img");
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task UploadDriverLicenseImage_ValidFlow_ReturnsSuccess()
        {
            var service = CreateService();
            var entity = new DeliveryPartnerEntity { Identifier = "id" };
            _repoMock.Setup(r => r.GetDeliveryPartnerByIdentifier("id")).ReturnsAsync(entity);
            _imageValidatorMock.Setup(v => v.ConvertBase64ImageToStream("id", "base64img"))
                .Returns((new MemoryStream(), "fileName.png", "image/png"));
            _minioMock.Setup(m => m.UploadFileAsync("fileName.png", It.IsAny<Stream>(), "image/png"))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateDeliveryPartner(entity)).ReturnsAsync(true);
            var result = await service.UploadDriverLicenseImage("id", "base64img");
            Assert.True(result.Success);
            Assert.Equal(Resource.DELIVERYRIDER_UPDATED_SUCCESSFULLY, result.Message.Message);
        }

        [Fact]
        public async Task UploadDriverLicenseImage_ImageValidatorCalled()
        {
            var service = CreateService();
            var entity = new DeliveryPartnerEntity { Identifier = "id" };
            _repoMock.Setup(r => r.GetDeliveryPartnerByIdentifier("id")).ReturnsAsync(entity);
            _imageValidatorMock.Setup(v => v.ConvertBase64ImageToStream("id", "base64img"))
                .Returns((new MemoryStream(), "fileName.png", "image/png"));
            _minioMock.Setup(m => m.UploadFileAsync("fileName.png", It.IsAny<Stream>(), "image/png"))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateDeliveryPartner(entity)).ReturnsAsync(true);
            await service.UploadDriverLicenseImage("id", "base64img");
            _imageValidatorMock.Verify(v => v.ConvertBase64ImageToStream("id", "base64img"), Times.Once);
        }

        [Fact]
        public async Task UploadDriverLicenseImage_UploadFileAsyncCalled()
        {
            var service = CreateService();
            var entity = new DeliveryPartnerEntity { Identifier = "id" };
            _repoMock.Setup(r => r.GetDeliveryPartnerByIdentifier("id")).ReturnsAsync(entity);
            _imageValidatorMock.Setup(v => v.ConvertBase64ImageToStream("id", "base64img"))
                .Returns((new MemoryStream(), "fileName.png", "image/png"));
            _minioMock.Setup(m => m.UploadFileAsync("fileName.png", It.IsAny<Stream>(), "image/png"))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateDeliveryPartner(entity)).ReturnsAsync(true);
            await service.UploadDriverLicenseImage("id", "base64img");
            _minioMock.Verify(m => m.UploadFileAsync("fileName.png", It.IsAny<Stream>(), "image/png"), Times.Once);
        }

        [Fact]
        public async Task RegisterDeliveryRider_DriverLicenseNumberExists_ReturnsFail_LogsWarning()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto
            {
                Identifier = "id",
                Name = "name",
                Cnpj = "09077683000150",
                DriverLicenseNumber = "12345678901",
                DriverLicenseType = "A",
                DateOfBirth = DateTime.Now
            };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(false);
            _repoMock.Setup(r => r.ExistsByCnh(dto.DriverLicenseNumber)).ReturnsAsync(true);

            var result = await service.RegisterDeliveryRider(dto);

            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterDeliveryRider_CnpjAlreadyExists_ReturnsFail_LogsWarning()
        {
            var service = CreateService();
            var dto = new DeliveryPartnerDto
            {
                Identifier = "id",
                Name = "name",
                Cnpj = "09077683000150",
                DriverLicenseNumber = "12345678901",
                DriverLicenseType = "A",
                DateOfBirth = DateTime.Now
            };
            _repoMock.Setup(r => r.ExistsByCnpj(dto.Cnpj)).ReturnsAsync(true);

            var result = await service.RegisterDeliveryRider(dto);

            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }
    }
}
