namespace MotoManager.IoC.DI
{
    /// <summary>
    /// Provides extension methods for registering worker dependencies related to motorcycle processing in the dependency injection container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WorkerMotocycleDependencyInjection
    {
        /// <summary>
        /// Registers worker services, repositories, event handlers, loggers, and MongoDB dependencies in the service collection.
        /// </summary>
        /// <param name="services">The service collection to add worker dependencies to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddWorkerDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton(typeof(BaseLogger<>), typeof(BaseLogger<>));
            services.AddSingleton<IRabbitMqEventHandler, RabbitMqEventHandler>();
            services.AddSingleton<IMongoClient>(_ => new MongoClient(MongoConstants.ConnectionString));
            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<IMotocycleRepository, MotocycleRepository>();

            return services;
        }
    }
}
