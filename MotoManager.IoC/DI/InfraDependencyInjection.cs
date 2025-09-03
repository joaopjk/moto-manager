namespace MotoManager.IoC.DI
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services and dependencies in the dependency injection container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class InfraDependencyInjection
    {
        /// <summary>
        /// Registers MongoDB, repositories, event handlers, loggers, and storage services in the service collection.
        /// </summary>
        /// <param name="services">The service collection to add infrastructure services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(MongoConstants.ConnectionString));
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IMotocycleRepository, MotocycleRepository>();
            services.AddScoped<IDeliveryPartnerRepository, DeliveryPartnerRepository>();
            services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();
            services.AddScoped<IMotocycleRentalRepository, MotocycleRentalRepository>();
            services.AddScoped<IRentalPlanTaxRepository, RentalPlanTaxRepository>();
            services.AddScoped<IRabbitMqEventHandler, RabbitMqEventHandler>();
            services.AddScoped<IMotocycleEventHandler, MotocycleEventHandler>();
            services.AddScoped(typeof(BaseLogger<>), typeof(BaseLogger<>));
            services.AddSingleton<IMinIoStorage>(_ => new MinIoStorage(
                MinIoConstants.Endpoint,
                MinIoConstants.AccessKey,
                MinIoConstants.SecretKey,
                MinIoConstants.BucketName));

            return services;
        }
    }
}
