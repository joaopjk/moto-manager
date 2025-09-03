namespace MotoManager.Domain.Interfaces.Events
{
    /// <summary>
    /// Interface for handling motorcycle creation events.
    /// </summary>
    public interface IMotocycleEventHandler
    {
        /// <summary>
        /// Publishes a motorcycle created event.
        /// </summary>
        /// <param name="@event">Motorcycle created event to publish.</param>
        /// <returns>True if the event was published successfully; otherwise, false.</returns>
        Task<bool> PublishMotocycleEvent(MotocycleCreatedEvent @event);
    }
}
