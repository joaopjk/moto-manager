namespace MotoManager.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for rental plan tax repository operations.
    /// </summary>
    public interface IRentalPlanTaxRepository
    {
        /// <summary>
        /// Retrieves all rental plan tax entities.
        /// </summary>
        /// <returns>List of rental plan tax entities.</returns>
        Task<List<RentalPlanTaxEntity>> GetAllAsync();

        /// <summary>
        /// Ensures that the default rental plan tax entities are present in the repository.
        /// </summary>
        /// <param name="defaultPlans">Collection of default rental plan tax entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EnsureDefaultRentalTaxPlansAsync(IEnumerable<RentalPlanTaxEntity> defaultPlans);
    }
}
