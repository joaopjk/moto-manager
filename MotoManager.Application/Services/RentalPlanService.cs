namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for rental plan operations, including price calculation and plan retrieval with caching.
    /// </summary>
    public class RentalPlanService(
        IRentalPlanRepository rentalPlanRepository,
        IDegradationCacheManager degradationCacheManager) : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository = rentalPlanRepository ?? throw new ArgumentException(nameof(rentalPlanRepository));
        private readonly IDegradationCacheManager _degradationCacheManager = degradationCacheManager ?? throw new ArgumentException(nameof(degradationCacheManager));

        /// <summary>
        /// Calculates the total rent value and daily rate for a given plan and number of days.
        /// Returns null values if the plan is not found.
        /// </summary>
        /// <param name="plan">Number of days for the rental plan.</param>
        /// <param name="days">Number of rental days.</param>
        /// <returns>Tuple containing total rent value and daily rate, or nulls if not found.</returns>
        public async Task<(decimal? totalRentValue, decimal? dailyRate)> GetRentalValue(int plan, int days)
        {
            var rentalPlan = await GetRentalPlan(plan);

            if (rentalPlan != null)
            {
                var totalRentValue = Math.Round(rentalPlan.DailyRate * days, 2);
                var dailyRate = Math.Round(rentalPlan.DailyRate, 2);

                return (totalRentValue, dailyRate);
            }

            return (null, null);
        }

        /// <summary>
        /// Retrieves the rental plan entity for the specified number of days, using cache when available.
        /// </summary>
        /// <param name="plan">Number of days for the rental plan.</param>
        /// <returns>The rental plan entity if found; otherwise, null.</returns>
        private async Task<RentalPlanEntity> GetRentalPlan(int plan)
        {
            const string cacheKey = "rental_plans";
            const int maxMemoryCacheTime = 10;
            const int maxRedisTime = 30;
            var rentalPlans = await _degradationCacheManager.GetOrSet(
                cacheKey,
                () => _rentalPlanRepository.GetAllRentalPlans(),
                TimeSpan.FromMinutes(maxMemoryCacheTime),
                TimeSpan.FromMinutes(maxRedisTime));

            var distinctPlans = rentalPlans
                .GroupBy(p => p.Days)
                .Select(g => g.First())
                .ToList();

            return distinctPlans.FirstOrDefault(p => p.Days == plan);
        }
    }
}
