namespace MotoManager.Domain.Interfaces.Caches
{
    /// <summary>
    /// Interface for cache management with degradation support (memory and Redis).
    /// </summary>
    public interface IDegradationCacheManager
    {
        /// <summary>
        /// Gets a value from cache or sets it using the provided fetch function, with optional expiry times.
        /// </summary>
        /// <typeparam name="T">Type of the value to cache.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="fetchFunc">Function to fetch the value if not in cache.</param>
        /// <param name="memoryExpiry">Optional memory cache expiry.</param>
        /// <param name="redisExpiry">Optional Redis cache expiry.</param>
        /// <returns>The cached or fetched value.</returns>
        Task<T> GetOrSet<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? memoryExpiry = null, TimeSpan? redisExpiry = null);
    }
}
