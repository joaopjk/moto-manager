namespace MotoManager.Application.Interfaces
{
    /// <summary>
    /// Interface for motorcycle service operations such as registration, retrieval, search, update, and deletion.
    /// </summary>
    public interface IMotocycleService
    {
        /// <summary>
        /// Registers a new motorcycle.
        /// </summary>
        /// <param name="dto">DTO containing motorcycle information.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        Task<Result<string>> RegisterMotocycle(MotocycleDto dto);

        /// <summary>
        /// Retrieves a motorcycle by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>Result containing the motorcycle DTO if found, or failure if not found.</returns>
        Task<Result<MotocycleDto>> GetMotocycleByIdentifier(string identifier);

        /// <summary>
        /// Searches for motorcycles by plate. If plate is empty, returns all motorcycles.
        /// </summary>
        /// <param name="plate">Plate to search for. If null or empty, returns all motorcycles.</param>
        /// <returns>Result containing a list of motorcycle DTOs.</returns>
        Task<Result<IEnumerable<MotocycleDto>>> SearchMotocycle(string plate);

        /// <summary>
        /// Updates the plate of a motorcycle identified by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <param name="dto">DTO containing the new plate information.</param>
        /// <returns>Result indicating success or failure of the update.</returns>
        Task<Result<string>> UpdatePlateMotocycle(string identifier, PlateDto dto);

        /// <summary>
        /// Deletes a motorcycle by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>Result indicating success or failure of the deletion.</returns>
        Task<Result<string>> DeleteMotocycle(string identifier);
    }
}
