using MotoManager.Domain.Events;

namespace MotoManager.Worker.MotocycleCreate.Services
{
    /// <summary>
    /// Interface for worker services that process MotocycleCreatedEvent events.
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Processes a MotocycleCreatedEvent, performing business logic or integration tasks.
        /// </summary>
        /// <param name="event">The MotocycleCreatedEvent to process. Can be null.</param>
        Task ProcessMotocycleCreatedEvent(MotocycleCreatedEvent? @event);
    }
}
