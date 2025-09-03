namespace MotoManager.Application.Interfaces
{
    public interface IDeliveryPartnerService
    {
        /// <summary>
        /// Registers a new delivery rider after validating the provided data and checking for duplicates.
        /// </summary>
        /// <param name="dto">Delivery partner data transfer object.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        Task<Result<string>> RegisterDeliveryRider(DeliveryPartnerDto dto);

        /// <summary>
        /// Uploads the CNH image for a delivery partner, validates the image, and updates the partner's record.
        /// </summary>
        /// <param name="id">Identifier of the delivery partner.</param>
        /// <param name="file">Base64 string of the CNH image.</param>
        /// <returns>Result indicating success or failure of the upload and update.</returns>
        Task<Result<string>> UploadDriverLicenseImage(string id, string file);
    }
}
