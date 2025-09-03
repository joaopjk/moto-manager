namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Represents a motorcycle rental entity stored in MongoDB.
    /// </summary>
    public class MotocycleRentalEntity : BaseMongoEntity
    {
        /// <summary>
        /// Unique identifier for the rental.
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Identifier of the delivery partner.
        /// </summary>
        public string IdDeliveryPartner { get; set; }
        /// <summary>
        /// Identifier of the motorcycle.
        /// </summary>
        public string IdMotocycle { get; set; }
        /// <summary>
        /// Start date of the rental.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// End date of the rental.
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Expected end date of the rental.
        /// </summary>
        public DateTime ExpectedEndDate { get; set; }
        /// <summary>
        /// Rental plan identifier.
        /// </summary>
        public int Plan { get; set; }
        /// <summary>
        /// Daily rent value for the rental.
        /// </summary>
        public decimal DailyRent { get; set; }
        /// <summary>
        /// Total rent value for the rental.
        /// </summary>
        public decimal TotalRentValue { get; set; }
    }
}
