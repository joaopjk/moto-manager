namespace MotoManager.Domain.AggregateRoots
{
    /// <summary>
    /// Aggregate root responsible for creating MotocycleCreatedEvent instances with validation.
    /// </summary>
    public static class MotocycleCreatedEventAggregateRoot
    {
        /// <summary>
        /// Creates a MotocycleCreatedEvent after validating all required fields.
        /// Throws ArgumentException if any field is invalid.
        /// </summary>
        /// <param name="correlationId">Correlation ID for tracking the event.</param>
        /// <param name="userId">User ID associated with the event.</param>
        /// <param name="identifier">Unique identifier for the motorcycle.</param>
        /// <param name="year">Year of the motorcycle.</param>
        /// <param name="model">Model of the motorcycle.</param>
        /// <param name="plate">Plate of the motorcycle.</param>
        /// <returns>A validated MotocycleCreatedEvent instance.</returns>
        public static MotocycleCreatedEvent CreateCreatedEvent(string correlationId, string userId, string identifier, int year, string model, string plate)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
                throw new ArgumentException("CorrelationId cannot be null or empty.", nameof(correlationId));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));
            if (year <= 0)
                throw new ArgumentException("Year must be greater than zero.", nameof(year));
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be null or empty.", nameof(model));
            if (string.IsNullOrWhiteSpace(plate))
                throw new ArgumentException("Plate cannot be null or empty.", nameof(plate));

            return new MotocycleCreatedEvent(
                correlationId,
                userId,
                identifier,
                year,
                model,
                plate
            );
        }
    }
}
