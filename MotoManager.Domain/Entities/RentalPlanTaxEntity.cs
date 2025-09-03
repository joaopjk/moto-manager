namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Represents a rental plan entity stored in MongoDB.
    /// </summary>
    public class RentalPlanTaxEntity : BaseMongoEntity
    {
        /// <summary>
        /// Number of days for the rental plan.
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// Daily rate for the rental plan.
        /// </summary>
        public decimal DailyTax { get; set; }
    }
}
