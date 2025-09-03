namespace MotoManager.Domain.Events
{
    /// <summary>
    /// Event representing the creation of a motorcycle, used for integration and messaging.
    /// </summary>
    public class MotocycleCreatedEvent
    {
        public MotocycleCreatedEvent() { }
        internal MotocycleCreatedEvent(string correlationId, string userId, string identifier, int year, string model, string plate)
        {
            CorrelationId = correlationId;
            UserId = userId;
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }

        /// <summary>
        /// Correlation ID for tracking the event.
        /// </summary>
        public string CorrelationId { get; set; }
        /// <summary>
        /// User ID associated with the event.
        /// </summary>
        public string UserId { get; set; }
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
