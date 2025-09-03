namespace MotoManager.Application.Interfaces
{
    /// <summary>
    /// Interface for rental plan service operations, including price calculation for rental plans.
    /// </summary>
    public interface IRentalPlanService
    {
        /// <summary>
        /// Calculates the total rent value and daily rate for a given plan and number of days.
        /// Returns null values if the plan is not found.
        /// </summary>
        /// <param name="plan">Number of days for the rental plan.</param>
        /// <param name="days">Number of rental days.</param>
        /// <returns>Tuple containing total rent value and daily rate, or nulls if not found.</returns>
        Task<(decimal? totalRentValue, decimal? dailyRate)> GetRentalValue(int plan, int days);
    }
}
