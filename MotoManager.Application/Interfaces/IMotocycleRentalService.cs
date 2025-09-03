namespace MotoManager.Application.Interfaces
{
    /// <summary>
    /// Interface for motorcycle rental service operations, including registration and retrieval.
    /// </summary>
    public interface IMotocycleRentalService
    {
        /// <summary>
        /// Registers a new motorcycle rental.
        /// </summary>
        /// <param name="createDto">DTO containing rental information.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        Task<Result<string>> RegisterRental(MotocycleRentalCreateDto createDto);

        /// <summary>
        /// Retrieves a motorcycle rental by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the rental.</param>
        /// <returns>Result containing the rental DTO if found, or failure if not found.</returns>
        Task<Result<MotocycleRentalDto>> GetMotocycleRentalByIdentifier(string identifier);
    }
}
