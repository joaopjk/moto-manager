namespace MotoManager.UnitTests.Worker
{
    /// <summary>
    /// Unit tests for the WorkerMotocycleCreateService.
    /// </summary>
    public class WorkerMotocycleCreateServiceTests
    {
        private readonly Mock<IMotocycleRepository> _motocycleRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<WorkerService>> _loggerMock = new();
        private readonly MotocycleCreatedEvent _evt = MotocycleCreatedEventAggregateRoot.CreateCreatedEvent(
            "corr", "user", " ID1", 2024, "Model", "ABC1234");

        private WorkerService CreateService()
        {
            var baseLogger = new BaseLogger<WorkerService>(_loggerMock.Object);
            return new WorkerService(_motocycleRepoMock.Object, _mapperMock.Object, baseLogger);
        }

        [Fact]
        public async Task ProcessMotocycleCreatedEvent_EventIsNull_DoesNotCallRepository()
        {
            var service = CreateService();
            await service.ProcessMotocycleCreatedEvent(null);
            _motocycleRepoMock.Verify(r => r.GetMotocycleByPlate(It.IsAny<string>()), Times.Never);
            _motocycleRepoMock.Verify(r => r.RegisterMotocycle(It.IsAny<MotocycleEntity>()), Times.Never);
        }

        [Fact]
        public async Task ProcessMotocycleCreatedEvent_MotocycleAlreadyExists_DoesNotRegisterMotocycle()
        {
            var service = CreateService();
            _motocycleRepoMock.Setup(r => r.GetMotocycleByPlate("ABC1234")).ReturnsAsync(new MotocycleEntity());
            await service.ProcessMotocycleCreatedEvent(_evt);
            _motocycleRepoMock.Verify(r => r.RegisterMotocycle(It.IsAny<MotocycleEntity>()), Times.Never);
        }

        [Fact]
        public async Task ProcessMotocycleCreatedEvent_RegisterSuccess_CallsRegisterMotocycle()
        {
            var service = CreateService();
            _motocycleRepoMock.Setup(r => r.GetMotocycleByPlate("ABC1234")).ReturnsAsync((MotocycleEntity)null);
            _mapperMock.Setup(m => m.Map<MotocycleEntity>(_evt)).Returns(new MotocycleEntity { Identifier = "id1" });

            await service.ProcessMotocycleCreatedEvent(_evt);

            _motocycleRepoMock.Verify(r => r.RegisterMotocycle(It.Is<MotocycleEntity>(m => m.Identifier == "id1")), Times.Once);
        }

        [Fact]
        public async Task ProcessMotocycleCreatedEvent_RegisterFails_CallsRegisterMotocycleOnce()
        {
            var service = CreateService();
            _motocycleRepoMock.Setup(r => r.GetMotocycleByPlate("ABC1234")).ReturnsAsync((MotocycleEntity)null);
            _mapperMock.Setup(m => m.Map<MotocycleEntity>(_evt)).Returns(new MotocycleEntity { Identifier = "id1" });
            _motocycleRepoMock.Setup(r => r.RegisterMotocycle(It.IsAny<MotocycleEntity>())).ReturnsAsync(false);

            await service.ProcessMotocycleCreatedEvent(_evt);

            _motocycleRepoMock.Verify(r => r.RegisterMotocycle(It.Is<MotocycleEntity>(m => m.Identifier == "id1")), Times.Once);
        }

        [Fact]
        public async Task ProcessMotocycleCreatedEvent_ExceptionThrown_DoesNotRegisterMotocycle()
        {
            var service = CreateService();
            _motocycleRepoMock.Setup(r => r.GetMotocycleByPlate("ABC1234")).ThrowsAsync(new Exception("db error"));
            await service.ProcessMotocycleCreatedEvent(_evt);
            _motocycleRepoMock.Verify(r => r.RegisterMotocycle(It.IsAny<MotocycleEntity>()), Times.Never);
        }
    }
}
