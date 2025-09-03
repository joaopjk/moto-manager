namespace MotoManager.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for motorcycle repository operations.
    /// </summary>
    public interface IMotocycleRepository
    {
        /// <summary>
        /// Retrieves a motorcycle entity by its plate.
        /// </summary>
        /// <param name="plate">Plate of the motorcycle.</param>
        /// <returns>The motorcycle entity if found; otherwise, null.</returns>
        Task<MotocycleEntity> GetMotocycleByPlate(string plate);
        /// <summary>
        /// Registers a new motorcycle entity.
        /// </summary>
        /// <param name="motocycle">Motorcycle entity to register.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        Task<bool> RegisterMotocycle(MotocycleEntity motocycle);
        /// <summary>
        /// Retrieves a motorcycle entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>The motorcycle entity if found; otherwise, null.</returns>
        Task<MotocycleEntity> GetMotocycleByIdentifier(string identifier);
        /// <summary>
        /// Retrieves all motorcycle entities.
        /// </summary>
        /// <returns>A collection of all motorcycle entities.</returns>
        Task<IEnumerable<MotocycleEntity>> GetAllMotocycle();
        /// <summary>
        /// Deletes a motorcycle entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>True if deletion is successful; otherwise, false.</returns>
        Task<bool> DeleteMotocycle(string identifier);

        /// <summary>
        /// Updates an existing motorcycle entity by its identifier.
        /// </summary>
        /// <param name="motocycle">Motorcycle entity with updated data.</param>
        /// <returns>True if update is successful; otherwise, false.</returns>
        Task<bool> UpdateMotocyclePlate(MotocycleEntity motocycle);
    }
}
