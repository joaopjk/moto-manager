namespace MotoManager.IntegrationTests.Caches
{
    /// <summary>
    /// Integration tests for MemoryCacheHelper using IMemoryCache.
    /// </summary>
    public class MemoryCacheHelperIntegrationTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheHelper _cacheHelper;

        public MemoryCacheHelperIntegrationTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cacheHelper = new MemoryCacheHelper(_memoryCache);
        }

        /// <summary>
        /// Verifies that GetOrSet stores and retrieves a value from memory cache.
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

        /// <summary>
        /// Verifies that GetOrSet respects cache expiry and fetches new value after expiration.
        /// </summary>
        [Fact]
        public async Task GetOrSet_ShouldExpireAndFetchNewValue()
        {
            string key = "expire-key";
            string value = "first-value";
            var result = await _cacheHelper.GetOrSet(key, () => Task.FromResult(value), TimeSpan.FromMilliseconds(100));
            Assert.Equal(value, result);

            await Task.Delay(150);

            var newValue = "second-value";
            var refreshed = await _cacheHelper.GetOrSet(key, () => Task.FromResult(newValue), TimeSpan.FromMinutes(1));
            Assert.Equal(newValue, refreshed);
        }
    }
}
