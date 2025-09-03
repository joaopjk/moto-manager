namespace MotoManager.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for delivery partner repository operations.
    /// </summary>
    public interface IDeliveryPartnerRepository
    {
        /// <summary>
        /// Checks if a delivery partner exists by CNPJ.
        /// </summary>
        /// <param name="cnpj">CNPJ to check.</param>
        /// <returns>True if exists; otherwise, false.</returns>
        Task<bool> ExistsByCnpj(string cnpj);
        /// <summary>
        /// Checks if a delivery partner exists by CNH.
        /// </summary>
        /// <param name="cnh">CNH to check.</param>
        /// <returns>True if exists; otherwise, false.</returns>
        Task<bool> ExistsByCnh(string cnh);
        /// <summary>
        /// Registers a new delivery partner entity.
        /// </summary>
        /// <param name="dto">Delivery partner entity to register.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        Task<bool> RegisterDeliveryPartner(DeliveryPartnerEntity dto);
        /// <summary>
        /// Retrieves a delivery partner entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the delivery partner.</param>
        /// <returns>The delivery partner entity if found; otherwise, null.</returns>
        Task<DeliveryPartnerEntity> GetDeliveryPartnerByIdentifier(string identifier);
        /// <summary>
        /// Updates a delivery partner entity.
        /// </summary>
        /// <param name="deliveryPartner">Delivery partner entity to update.</param>
        /// <returns>True if update is successful; otherwise, false.</returns>
        Task<bool> UpdateDeliveryPartner(DeliveryPartnerEntity deliveryPartner);
    }
}
