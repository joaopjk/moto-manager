namespace MotoManager.Infrastructure.Caches
{
    /// <summary>
    /// Helper for Redis cache operations.
    /// </summary>
    public class RedisCacheHelper(IConnectionMultiplexer connection) : IRedisCacheHelper
    {
        private readonly IDatabase _db = connection.GetDatabase();

        /// <summary>
        /// Gets a value from Redis cache or sets it using the provided fetch function, with optional expiry.
        /// </summary>
        public async Task<T> GetOrSet<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? expiry = null)
        {
            var cached = await _db.StringGetAsync(key);
            if (cached.HasValue)
                return System.Text.Json.JsonSerializer.Deserialize<T>(cached);

            var value = await fetchFunc();
            if (value != null)
            {
                var serialized = System.Text.Json.JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, serialized, expiry);
            }
            return value;
        }
    }
}
