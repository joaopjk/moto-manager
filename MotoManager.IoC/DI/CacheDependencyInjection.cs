namespace MotoManager.IoC.DI
{
    /// <summary>
    /// Provides extension methods for registering cache services and dependencies in the dependency injection container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CacheDependencyInjection
    {
        /// <summary>
        /// Registers Redis, memory cache, and related cache helpers in the service collection.
        /// </summary>
        /// <param name="services">The service collection to add cache services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(RedisConstants.ConnectionString));
            services.AddSingleton<IRedisCacheHelper, RedisCacheHelper>();
            services.AddMemoryCache();
            services.AddSingleton<IMemoryCacheHelper, MemoryCacheHelper>();
            services.AddSingleton<IDegradationCacheManager, DegradationCacheManager>();
            return services;
        }
    }
}
