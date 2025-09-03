namespace MotoManager.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for rental plan repository operations.
    /// </summary>
    public interface IRentalPlanRepository
    {
        /// <summary>
        /// Registers a new rental plan entity.
        /// </summary>
        /// <param name="entity">Rental plan entity to register.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        Task<bool> RegisterRentalPlan(RentalPlanEntity entity);
        /// <summary>
        /// Ensures that the default rental plans are present in the repository.
        /// </summary>
        /// <param name="defaultPlans">Collection of default rental plans.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EnsureDefaultRentalPlans(IEnumerable<RentalPlanEntity> defaultPlans);
        /// <summary>
        /// Retrieves all rental plan entities.
        /// </summary>
        /// <returns>A collection of all rental plan entities.</returns>
        Task<IEnumerable<RentalPlanEntity>> GetAllRentalPlans();
    }
}
