namespace MotoManager.IntegrationTests.Repositories
{
    /// <summary>
    /// Integration tests for DeliveryPartnerRepository using a real MongoDB container via TestContainers.
    /// </summary>
    public class DeliveryPartnerRepositoryIntegrationTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private IMongoClient _client = null!;
        private MongoContext _context = null!;
        private DeliveryPartnerRepository _repository = null!;

        public DeliveryPartnerRepositoryIntegrationTests()
        {
            _mongoContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0.5")
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            Environment.SetEnvironmentVariable("MONGO_DATABASE_NAME", "MONGO_DATABASE_NAME");
            await _mongoContainer.StartAsync();
            _client = new MongoClient(_mongoContainer.GetConnectionString());
            _context = new MongoContext(_client);
            _repository = new DeliveryPartnerRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact]
        public async Task RegisterAndGetDeliveryPartner_ShouldPersistAndRetrieve()
        {
            var entity = new DeliveryPartnerEntity
            {
                Identifier = "id-1",
                Name = "Partner One",
                Cnpj = "12345678000199",
                DateOfBirth = new DateTime(1990, 1, 1),
                DriverLicenseNumber = "CNH123456",
                ImageDriverLicenseNumberFileName = "cnh.jpg",
                DriverLicenseType = "A"
            };
            var registered = await _repository.RegisterDeliveryPartner(entity);
            Assert.True(registered);

            var found = await _repository.GetDeliveryPartnerByIdentifier("id-1");
            Assert.NotNull(found);
            Assert.Equal("Partner One", found.Name);
        }

        [Fact]
        public async Task ExistsByCnpj_ShouldReturnTrueIfExists()
        {
            var entity = new DeliveryPartnerEntity
            {
                Identifier = "id-2",
                Name = "Partner Two",
                Cnpj = "98765432000188",
                DateOfBirth = new DateTime(1985, 5, 5),
                DriverLicenseNumber = "CNH654321",
                ImageDriverLicenseNumberFileName = "cnh2.jpg",
                DriverLicenseType = "B"
            };
            await _repository.RegisterDeliveryPartner(entity);
            var exists = await _repository.ExistsByCnpj("98765432000188");
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByCnh_ShouldReturnTrueIfExists()
        {
            var entity = new DeliveryPartnerEntity
            {
                Identifier = "id-3",
                Name = "Partner Three",
                Cnpj = "11223344000177",
                DateOfBirth = new DateTime(1980, 10, 10),
                DriverLicenseNumber = "CNH777888",
                ImageDriverLicenseNumberFileName = "cnh3.jpg",
                DriverLicenseType = "C"
            };
            await _repository.RegisterDeliveryPartner(entity);
            var exists = await _repository.ExistsByCnh("CNH777888");
            Assert.True(exists);
        }

        [Fact]
        public async Task UpdateDeliveryPartner_ShouldUpdateEntity()
        {
            var entity = new DeliveryPartnerEntity
            {
                Identifier = "id-4",
                Name = "Partner Four",
                Cnpj = "55667788000166",
                DateOfBirth = new DateTime(1995, 3, 3),
                DriverLicenseNumber = "CNH999000",
                ImageDriverLicenseNumberFileName = "cnh4.jpg",
                DriverLicenseType = "D"
            };
            await _repository.RegisterDeliveryPartner(entity);

            entity.Name = "Partner Four Updated";
            var updated = await _repository.UpdateDeliveryPartner(entity);
            Assert.True(updated);

            var found = await _repository.GetDeliveryPartnerByIdentifier("id-4");
            Assert.NotNull(found);
            Assert.Equal("Partner Four Updated", found.Name);
        }
    }
}
