namespace MotoManager.IoC.DI
{
    /// <summary>
    /// Provides extension methods for registering AutoMapper profiles and configuration in the dependency injection container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AutoMapperDependencyInjection
    {
        /// <summary>
        /// Registers AutoMapper and its profiles in the service collection.
        /// </summary>
        /// <param name="services">The service collection to add AutoMapper to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MotocycleProfile>(); ;
                    cfg.AddProfile<DeliveryRiderProfile>();
                    cfg.AddProfile<MotocycleRentalProfile>();
                }, loggerFactory);

                return config.CreateMapper();
            });

            return services;
        }
    }
}
