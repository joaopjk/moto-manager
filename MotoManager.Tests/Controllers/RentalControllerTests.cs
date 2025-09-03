using MotoManager.Api.Controllers.V1;

namespace MotoManager.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for the RentalController.
    /// </summary>
    public class RentalControllerTests
    {
        private readonly Mock<IMotocycleRentalService> _serviceMock = new();
        private RentalController CreateController() => new RentalController(_serviceMock.Object);

        [Fact]
        public async Task RegisterRental_Success_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RegisterRental(It.IsAny<MotocycleRentalCreateDto>())).ReturnsAsync(Result<string>.Ok());
            var controller = CreateController();
            var result = await controller.RegisterRental(new MotocycleRentalCreateDto());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterRental_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.RegisterRental(It.IsAny<MotocycleRentalCreateDto>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.RegisterRental(new MotocycleRentalCreateDto());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetRental_Success_ReturnsOkWithValue()
        {
            _serviceMock.Setup(s => s.GetMotocycleRentalByIdentifier(It.IsAny<string>())).ReturnsAsync(Result<MotocycleRentalDto>.Ok(new MotocycleRentalDto()));
            var controller = CreateController();
            var result = await controller.GetRental("id");
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<MotocycleRentalDto>(okResult.Value);
        }

        [Fact]
        public async Task GetRental_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.GetMotocycleRentalByIdentifier(It.IsAny<string>())).ReturnsAsync(Result<MotocycleRentalDto>.Fail("error"));
            var controller = CreateController();
            var result = await controller.GetRental("id");
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
