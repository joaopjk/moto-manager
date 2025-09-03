namespace MotoManager.IntegrationTests.Context
{
    /// <summary>
    /// Integration tests for MongoContext using a real MongoDB container via TestContainers.
    /// </summary>
    public class MongoContextIntegrationTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private IMongoClient _client = null!;
        private MongoContext _context = null!;

        public MongoContextIntegrationTests()
        {
            _mongoContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0.5")
                .WithCleanUp(true)
                .Build();
        }

        /// <summary>
        /// Initializes the MongoDB container and MongoContext before each test.
        /// </summary>
        public async Task InitializeAsync()
        {
            Environment.SetEnvironmentVariable("MONGO_DATABASE_NAME", "MONGO_DATABASE_NAME");
            await _mongoContainer.StartAsync();
            _client = new MongoClient(_mongoContainer.GetConnectionString());
            _context = new MongoContext(_client);
            
        }

        /// <summary>
        /// Disposes the MongoDB container after each test.
        /// </summary>
        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        /// <summary>
        /// Verifies that GetCollection returns a valid collection and allows CRUD operations.
        /// </summary>
        [Fact]
        public async Task GetCollection_ShouldInsertAndReadDocument()
        {
            var collection = _context.GetCollection<TestDocument>("TestDocuments");
            var doc = new TestDocument { Id = "1", Name = "TestName" };
            await collection.InsertOneAsync(doc);

            var found = await collection.Find(x => x.Id == "1").FirstOrDefaultAsync();
            Assert.NotNull(found);
            Assert.Equal("TestName", found.Name);
        }
    }
}
