namespace MotoManager.IoC.DI
{
    /// <summary>
    /// Provides extension methods for registering application services in the dependency injection container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceDependencyInjection
    {
        /// <summary>
        /// Registers application services such as domain services, validators, and helpers in the service collection.
        /// </summary>
        /// <param name="services">The service collection to add application services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMotocycleService, MotocycleService>();
            services.AddScoped<IDeliveryPartnerService, DeliveryPartnerService>();
            services.AddScoped<IMotocycleRentalService, MotocycleRentalService>();
            services.AddScoped<IRentalPlanService, RentalPlanService>();
            services.AddScoped<IHeaderService, HeaderService>();
            services.AddScoped<IImageValidatorService, ImageValidatorService>();

            return services;
        }
    }
}
