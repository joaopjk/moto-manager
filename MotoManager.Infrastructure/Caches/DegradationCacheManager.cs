namespace MotoManager.Infrastructure.Caches
{
    /// <summary>
    /// Helper for cache management with degradation support (memory and Redis).
    /// </summary>
    public class DegradationCacheManager(IMemoryCacheHelper memoryCache, IRedisCacheHelper redisCache) : IDegradationCacheManager
    {
        private readonly IMemoryCacheHelper _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        private readonly IRedisCacheHelper _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));

        /// <summary>
        /// Gets a value from memory cache or Redis cache, or sets it using the provided fetch function, with optional expiry times.
        /// </summary>
        public async Task<T> GetOrSet<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? memoryExpiry = null, TimeSpan? redisExpiry = null)
        {
            var value = await _memoryCache.GetOrSet(key, async () =>
                          await _redisCache.GetOrSet(key, fetchFunc, redisExpiry),
                memoryExpiry);
            return value;
        }
    }
}
