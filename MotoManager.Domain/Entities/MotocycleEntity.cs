namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Represents a motorcycle entity stored in MongoDB.
    /// </summary>
    public class MotocycleEntity : BaseMongoEntity
    {
        /// <summary>
        /// Unique identifier for the motorcycle.
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Year of the motorcycle.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Model of the motorcycle.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Plate of the motorcycle.
        /// </summary>
        public string Plate { get; set; }
    }
}
