namespace MotoManager.Domain.Interfaces.Caches
{
    /// <summary>
    /// Interface for Redis cache helper operations.
    /// </summary>
    public interface IRedisCacheHelper
    {
        /// <summary>
        /// Gets a value from Redis cache or sets it using the provided fetch function, with optional expiry.
        /// </summary>
        /// <typeparam name="T">Type of the value to cache.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="fetchFunc">Function to fetch the value if not in cache.</param>
        /// <param name="expiry">Optional cache expiry.</param>
        /// <returns>The cached or fetched value.</returns>
        Task<T> GetOrSet<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? expiry = null);
    }
}
