namespace MotoManager.IntegrationTests.Repositories
{
    /// <summary>
    /// Integration tests for RentalPlanRepository using a real MongoDB container via TestContainers.
    /// </summary>
    public class RentalPlanRepositoryIntegrationTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private IMongoClient _client = null!;
        private MongoContext _context = null!;
        private RentalPlanRepository _repository = null!;

        public RentalPlanRepositoryIntegrationTests()
        {
            _mongoContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0.5")
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();
            _client = new MongoClient(_mongoContainer.GetConnectionString());
            _context = new MongoContext(_client);
            _repository = new RentalPlanRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact]
        public async Task RegisterAndGetAllRentalPlans_ShouldPersistAndRetrieve()
        {
            var entity = new RentalPlanEntity
            {
                Days = 7,
                DailyRate = 50.0m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = "user-1"
            };
            var registered = await _repository.RegisterRentalPlan(entity);
            Assert.True(registered);

            var plans = await _repository.GetAllRentalPlans();
            Assert.NotEmpty(plans);
            Assert.Contains(plans, p => p.Days == 7 && p.DailyRate == 50.0m);
        }

        [Fact]
        public async Task EnsureDefaultRentalPlansAsync_ShouldInsertDefaultsIfEmpty()
        {
            var defaultPlans = new List<RentalPlanEntity>
            {
                new RentalPlanEntity { Days = 3, DailyRate = 30.0m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UserId = "user-2" },
                new RentalPlanEntity { Days = 5, DailyRate = 40.0m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UserId = "user-3" }
            };
            await _repository.EnsureDefaultRentalPlans(defaultPlans);
            var plans = await _repository.GetAllRentalPlans();
            Assert.True(plans.Count() >= 2);
            Assert.Contains(plans, p => p.Days == 3 && p.DailyRate == 30.0m);
            Assert.Contains(plans, p => p.Days == 5 && p.DailyRate == 40.0m);
        }
    }
}
