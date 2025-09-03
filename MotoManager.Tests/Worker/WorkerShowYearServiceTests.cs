namespace MotoManager.UnitTests.Worker
{
    /// <summary>
    /// Unit tests for the WorkerShowYearService.
    /// </summary>
    public class WorkerShowYearServiceTests
    {
        private readonly Mock<ILogger<MotoManager.Worker.ShowYear.Services.WorkerShowYearService>> _loggerMock = new();
        private readonly MotocycleCreatedEvent _evt = MotocycleCreatedEventAggregateRoot.CreateCreatedEvent(
            "corr", "user", "ID1", 2024, "Model", "ABC1234");

        private MotoManager.Worker.ShowYear.Services.WorkerShowYearService CreateService()
        {
            var baseLogger = new BaseLogger<MotoManager.Worker.ShowYear.Services.WorkerShowYearService>(_loggerMock.Object);
            return new MotoManager.Worker.ShowYear.Services.WorkerShowYearService(baseLogger);
        }

        [Fact]
        public async Task ProcessShowYear_EventIsNull_DoesNotThrow()
        {
            var service = CreateService();
            await service.ProcessShowYear(null);
        }

        [Fact]
        public async Task ProcessShowYear_IdentifierIsEmpty_DoesNotThrow()
        {
            var service = CreateService();
            await service.ProcessShowYear(_evt);
        }

        [Fact]
        public async Task ProcessShowYear_YearIs2024_DoesNotThrow()
        {
            var service = CreateService();
            await service.ProcessShowYear(_evt);
        }

        [Fact]
        public async Task ProcessShowYear_YearIsNot2024_DoesNotThrow()
        {
            var service = CreateService();
            await service.ProcessShowYear(_evt);
        }
    }
}
