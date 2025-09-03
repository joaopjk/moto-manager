using Microsoft.Extensions.Caching.Memory;

namespace MotoManager.Infrastructure.Caches
{
    /// <summary>
    /// Helper for memory cache operations.
    /// </summary>
    public class MemoryCacheHelper(IMemoryCache cache) : IMemoryCacheHelper
    {
        private readonly IMemoryCache _cache = cache ?? throw new ArgumentException(nameof(cache));

        /// <summary>
        /// Gets a value from memory cache or sets it using the provided fetch function, with optional expiry.
        /// </summary>
        public async Task<T> GetOrSet<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? expiry = null)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;

            value = await fetchFunc();
            if (value != null)
            {
                var options = new MemoryCacheEntryOptions();
                if (expiry.HasValue)
                    options.SetAbsoluteExpiration(expiry.Value);
                _cache.Set(key, value, options);
            }
            return value;
        }
    }
}
