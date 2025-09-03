namespace MotoManager.Api.Seeds
{
    /// <summary>
    /// Provides methods to seed default rental plans into the database during application startup.
    /// </summary>
    public static class RentalPlansSeed
    {
        /// <summary>
        /// Seeds the default rental plans into the repository if they do not exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedRentalPlansAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRentalPlanRepository>();
            var defaultPlans = new[]
            {
                new RentalPlanEntity { Days = 7, DailyRate = 30m },
                new RentalPlanEntity { Days = 15, DailyRate = 28m },
                new RentalPlanEntity { Days = 30, DailyRate = 22m },
                new RentalPlanEntity { Days = 45, DailyRate = 20m },
                new RentalPlanEntity { Days = 50, DailyRate = 18m }
            };
            await repo.EnsureDefaultRentalPlans(defaultPlans);

            var repo2 = scope.ServiceProvider.GetRequiredService<IRentalPlanTaxRepository>();
            var defaultTaxPlans = new[]
            {
                new RentalPlanTaxEntity() { Days = 7, DailyTax = 20 },
                new RentalPlanTaxEntity() { Days = 15, DailyTax = 40 }
            };
            await repo2.EnsureDefaultRentalTaxPlansAsync(defaultTaxPlans);
        }
    }
}
