namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the RentalPlanService class.
    /// </summary>
    public class RentalPlanServiceTests
    {
        private readonly Mock<IRentalPlanRepository> _rentalPlanRepoMock = new();
        private readonly Mock<IDegradationCacheManager> _cacheManagerMock = new();

        private RentalPlanService CreateService()
        {
            return new RentalPlanService(_rentalPlanRepoMock.Object, _cacheManagerMock.Object);
        }

        [Fact]
        public async Task GetPriceValue_ReturnsCorrectValues()
        {
            var service = CreateService();
            var plans = new List<RentalPlanEntity>
            {
                new RentalPlanEntity { Days = 5, DailyRate = 10 },
                new RentalPlanEntity { Days = 10, DailyRate = 8 }
            };
            _cacheManagerMock.Setup(c => c.GetOrSet(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<RentalPlanEntity>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()
            )).ReturnsAsync(plans);

            var (total, daily) = await service.GetRentalValue(10, 10);
            Assert.Equal(80, total);
            Assert.Equal(8, daily);
        }

        [Fact]
        public async Task GetPriceValue_PlanNotFound_ReturnsDefaultValues()
        {
            var service = CreateService();
            var plans = new List<RentalPlanEntity>
            {
                new RentalPlanEntity { Days = 5, DailyRate = 10 }
            };
            _cacheManagerMock.Setup(c => c.GetOrSet(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<RentalPlanEntity>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()
            )).ReturnsAsync(plans);

            var (total, daily) = await service.GetRentalValue(99, 99);
            Assert.Null(total);
            Assert.Null(daily);
        }

        [Fact]
        public async Task GetPriceValue_RoundsValues()
        {
            var service = CreateService();
            var plans = new List<RentalPlanEntity>
            {
                new RentalPlanEntity { Days = 3, DailyRate = 7m }
            };
            _cacheManagerMock.Setup(c => c.GetOrSet(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<RentalPlanEntity>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()
            )).ReturnsAsync(plans);

            var (total, daily) = await service.GetRentalValue(3, 3);
            Assert.Equal(21m, total);
            Assert.Equal(7m, daily);
        }
    }
}
