namespace MotoManager.IntegrationTests.Repositories
{
    /// <summary>
    /// Integration tests for MotocycleRentalRepository using a real MongoDB container via TestContainers.
    /// </summary>
    public class MotocycleRentalRepositoryIntegrationTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private IMongoClient _client = null!;
        private MongoContext _context = null!;
        private MotocycleRentalRepository _repository = null!;

        public MotocycleRentalRepositoryIntegrationTests()
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
            _repository = new MotocycleRentalRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact]
        public async Task RegisterAndGetMotocycleRental_ShouldPersistAndRetrieve()
        {
            var entity = new MotocycleRentalEntity
            {
                Identifier = "rental-1",
                IdMotocycle = "moto-1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2)
            };
            var registered = await _repository.RegisterMotocycleRental(entity);
            Assert.True(registered);

            var found = await _repository.GetMotocycleRentalByIdentifier("rental-1");
            Assert.NotNull(found);
            Assert.Equal("moto-1", found.IdMotocycle);
        }

        [Fact]
        public async Task GetMotocycleRentalByMotocycleId_ShouldReturnRental()
        {
            var entity = new MotocycleRentalEntity
            {
                Identifier = "rental-2",
                IdMotocycle = "moto-2",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2)
            };
            await _repository.RegisterMotocycleRental(entity);
            var found = await _repository.GetMotocycleRentalByMotocycleId("moto-2");
            Assert.NotNull(found);
            Assert.Equal("rental-2", found.Identifier);
        }
    }
}
