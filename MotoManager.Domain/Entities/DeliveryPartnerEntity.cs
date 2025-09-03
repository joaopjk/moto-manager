namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Represents a delivery partner entity stored in MongoDB.
    /// </summary>
    public class DeliveryPartnerEntity : BaseMongoEntity
    {
        /// <summary>
        /// Unique identifier for the delivery partner.
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Name of the delivery partner.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// CNPJ of the delivery partner.
        /// </summary>
        public string Cnpj { get; set; }
        /// <summary>
        /// Date of birth of the delivery partner.
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Driver license number (CNH) of the delivery partner.
        /// </summary>
        public string DriverLicenseNumber { get; set; }
        /// <summary>
        /// File name of the driver's license image.
        /// </summary>
        public string ImageDriverLicenseNumberFileName { get; set; }
        /// <summary>
        /// Driver license type (CNH type) of the delivery partner.
        /// </summary>
        public string DriverLicenseType { get; set; }
    }
}
