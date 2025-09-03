using MotoManager.Api.Controllers.V1;

namespace MotoManager.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for the MotocyclesController.
    /// </summary>
    public class MotocyclesControllerTests
    {
        private readonly Mock<IMotocycleService> _serviceMock = new();
        private MotocyclesController CreateController() => new MotocyclesController(_serviceMock.Object);

        [Fact]
        public async Task RegisterMotocycle_Success_ReturnsCreated()
        {
            _serviceMock.Setup(s => s.RegisterMotocycle(It.IsAny<MotocycleDto>())).ReturnsAsync(Result<string>.Ok());
            var controller = CreateController();
            var result = await controller.RegisterMotocycle(new MotocycleDto());
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task RegisterMotocycle_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.RegisterMotocycle(It.IsAny<MotocycleDto>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.RegisterMotocycle(new MotocycleDto());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetMotocycleByIdentifier_Success_ReturnsOkWithValue()
        {
            _serviceMock.Setup(s => s.GetMotocycleByIdentifier(It.IsAny<string>())).ReturnsAsync(Result<MotocycleDto>.Ok(new MotocycleDto()));
            var controller = CreateController();
            var result = await controller.GetMotocycleByIdentifier("id");
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<MotocycleDto>(okResult.Value);
        }

        [Fact]
        public async Task GetMotocycleByIdentifier_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.GetMotocycleByIdentifier(It.IsAny<string>())).ReturnsAsync(Result<MotocycleDto>.Fail("error"));
            var controller = CreateController();
            var result = await controller.GetMotocycleByIdentifier("id");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetMotocycle_Success_ReturnsOkWithValue()
        {
            _serviceMock.Setup(s => s.SearchMotocycle(It.IsAny<string>())).ReturnsAsync(Result<IEnumerable<MotocycleDto>>.Ok(new List<MotocycleDto>()));
            var controller = CreateController();
            var result = await controller.GetMotocycle("ABC1234");
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<MotocycleDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetMotocycle_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.SearchMotocycle(It.IsAny<string>())).ReturnsAsync(Result<IEnumerable<MotocycleDto>>.Fail("error"));
            var controller = CreateController();
            var result = await controller.GetMotocycle("ABC1234");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePlateMotocycle_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.UpdatePlateMotocycle(It.IsAny<string>(), It.IsAny<PlateDto>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.UpdatePlateMotocycle("id", new PlateDto());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMotocycle_Success_ReturnsOk()
        {
            _serviceMock.Setup(s => s.DeleteMotocycle(It.IsAny<string>())).ReturnsAsync(Result<string>.Ok());
            var controller = CreateController();
            var result = await controller.DeleteMotocycle("id");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteMotocycle_Fail_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.DeleteMotocycle(It.IsAny<string>())).ReturnsAsync(Result<string>.Fail("error"));
            var controller = CreateController();
            var result = await controller.DeleteMotocycle("id");
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
