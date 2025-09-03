namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Represents a rental plan entity stored in MongoDB.
    /// </summary>
    public class RentalPlanEntity : BaseMongoEntity
    {
        /// <summary>
        /// Number of days for the rental plan.
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// Daily rate for the rental plan.
        /// </summary>
        public decimal DailyRate { get; set; }
    }
}
