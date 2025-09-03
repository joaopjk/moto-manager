namespace MotoManager.IntegrationTests.Repositories
{
    /// <summary>
    /// Integration tests for MotocycleRepository using a real MongoDB container via TestContainers.
    /// </summary>
    public class MotocycleRepositoryIntegrationTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private IMongoClient _client = null!;
        private MongoContext _context = null!;
        private MotocycleRepository _repository = null!;

        public MotocycleRepositoryIntegrationTests()
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
            _repository = new MotocycleRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact]
        public async Task RegisterAndGetMotocycle_ShouldPersistAndRetrieve()
        {
            var entity = new MotocycleEntity
            {
                Identifier = "moto-1",
                Plate = "ABC1234",
                Model = "ModelX",
                Year = 2020
            };
            var registered = await _repository.RegisterMotocycle(entity);
            Assert.True(registered);

            var found = await _repository.GetMotocycleByIdentifier("moto-1");
            Assert.NotNull(found);
            Assert.Equal("ABC1234", found.Plate);
        }

        [Fact]
        public async Task GetMotocycleByPlate_ShouldReturnMotocycle()
        {
            var entity = new MotocycleEntity
            {
                Identifier = "moto-2",
                Plate = "XYZ5678",
                Model = "ModelY",
                Year = 2021
            };
            await _repository.RegisterMotocycle(entity);
            var found = await _repository.GetMotocycleByPlate("XYZ5678");
            Assert.NotNull(found);
            Assert.Equal("moto-2", found.Identifier);
        }

        [Fact]
        public async Task UpdateMotocyclePlate_ShouldUpdatePlate()
        {
            var entity = new MotocycleEntity
            {
                Identifier = "moto-3",
                Plate = "OLDPLATE",
                Model = "ModelZ",
                Year = 2022
            };
            await _repository.RegisterMotocycle(entity);
            entity.Plate = "NEWPLATE";
            var updated = await _repository.UpdateMotocyclePlate(entity);
            Assert.True(updated);
            var found = await _repository.GetMotocycleByIdentifier("moto-3");
            Assert.Equal("NEWPLATE", found.Plate);
        }

        [Fact]
        public async Task DeleteMotocycle_ShouldRemoveEntity()
        {
            var entity = new MotocycleEntity
            {
                Identifier = "moto-4",
                Plate = "DELPLATE",
                Model = "ModelA",
                Year = 2023
            };
            await _repository.RegisterMotocycle(entity);
            var deleted = await _repository.DeleteMotocycle("moto-4");
            Assert.True(deleted);
            var found = await _repository.GetMotocycleByIdentifier("moto-4");
            Assert.Null(found);
        }
    }
}
