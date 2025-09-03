namespace MotoManager.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for motorcycle rental repository operations.
    /// </summary>
    public interface IMotocycleRentalRepository
    {
        /// <summary>
        /// Registers a new motorcycle rental entity.
        /// </summary>
        /// <param name="entity">Motorcycle rental entity to register.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        Task<bool> RegisterMotocycleRental(MotocycleRentalEntity entity);
        /// <summary>
        /// Retrieves a motorcycle rental entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the rental.</param>
        /// <returns>The motorcycle rental entity if found; otherwise, null.</returns>
        Task<MotocycleRentalEntity> GetMotocycleRentalByIdentifier(string identifier);

        /// <summary>
        /// Retrieves a motorcycle rental entity by the motorcycle identifier.
        /// </summary>
        /// <param name="idMotocycle">Identifier of the motorcycle.</param>
        /// <returns>The motorcycle rental entity if found; otherwise, null.</returns>
        Task<MotocycleRentalEntity> GetMotocycleRentalByMotocycleId(string idMotocycle);
    }
}
