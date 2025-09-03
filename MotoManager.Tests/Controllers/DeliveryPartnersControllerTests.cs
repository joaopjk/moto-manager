using MotoManager.Api.Controllers.V1;

namespace MotoManager.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for the DeliveryPartnersController.
    /// </summary>
    public class DeliveryPartnersControllerTests
    {
        private readonly Mock<IDeliveryPartnerService> _serviceMock = new();
        private DeliveryPartnersController CreateController() => new DeliveryPartnersController(_serviceMock.Object);

        [Fact]
        public async Task RegisterDeliveryRider_Success_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RegisterDeliveryRider(It.IsAny<DeliveryPartnerDto>())).ReturnsAsync(Result<string>.Ok());
            var controller = CreateController();
            var result = await controller.RegisterDeliveryRider(new DeliveryPartnerDto());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterDeliveryRider_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.RegisterDeliveryRider(It.IsAny<DeliveryPartnerDto>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.RegisterDeliveryRider(new DeliveryPartnerDto());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadCnhImage_Success_ReturnsOk()
        {
            _serviceMock.Setup(s => s.UploadDriverLicenseImage(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Ok());
            var controller = CreateController();
            var result = await controller.UploadCnhImage("id", new ImageDriverLicenseNumberDto { ImageDriverLicenseNumber = "base64" });
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UploadCnhImage_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.UploadDriverLicenseImage(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.UploadCnhImage("id", new ImageDriverLicenseNumberDto { ImageDriverLicenseNumber = "base64" });
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
