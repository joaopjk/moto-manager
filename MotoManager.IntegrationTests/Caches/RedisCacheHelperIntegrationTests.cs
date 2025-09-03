namespace MotoManager.IntegrationTests.Caches
{
    /// <summary>
    /// Integration tests for RedisCacheHelper using a real Redis container via TestContainers.
    /// </summary>
    public class RedisCacheHelperIntegrationTests : IAsyncLifetime
    {
        private readonly RedisContainer _redisContainer;
        private IConnectionMultiplexer _connection = null!;
        private RedisCacheHelper _cacheHelper = null!;

        public RedisCacheHelperIntegrationTests()
        {
            _redisContainer = new RedisBuilder()
                .WithImage("redis:7.2-alpine")
                .WithCleanUp(true)
                .Build();
        }

        /// <summary>
        /// Initializes the Redis container and RedisCacheHelper before each test.
        /// </summary>
        public async Task InitializeAsync()
        {
            await _redisContainer.StartAsync();
            _connection = await ConnectionMultiplexer.ConnectAsync(_redisContainer.GetConnectionString());
            _cacheHelper = new RedisCacheHelper(_connection);
        }

        /// <summary>
        /// Disposes the Redis connection and container after each test.
        /// </summary>
        public async Task DisposeAsync()
        {
            await _connection.CloseAsync();
            await _redisContainer.DisposeAsync();
        }

        /// <summary>
        /// Verifies that GetOrSet stores and retrieves a value from Redis cache.
        /// </summary>
        [Fact]
        public async Task GetOrSet_ShouldSetAndGetValue()
        {
            string key = "test-key";
            string value = "test-value";
            var result = await _cacheHelper.GetOrSet(key, () => Task.FromResult(value), TimeSpan.FromMinutes(1));
            Assert.Equal(value, result);

            var cached = await _cacheHelper.GetOrSet(key, () => Task.FromResult("other-value"), TimeSpan.FromMinutes(1));
            Assert.Equal(value, cached);
        }

        /// <summary>
        /// Verifies that GetOrSet returns null when the fetch function returns null.
        /// </summary>
        [Fact]
        public async Task GetOrSet_ShouldReturnNullIfFetchReturnsNull()
        {
            string key = "null-key";
            var result = await _cacheHelper.GetOrSet<string>(key, () => Task.FromResult<string>(null), TimeSpan.FromMinutes(1));
            Assert.Null(result);
        }
    }
}
