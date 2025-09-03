using MotoManager.Domain.Events;

namespace MotoManager.Worker.ShowYear.Services
{
    /// <summary>
    /// Interface for worker services that process MotocycleCreatedEvent events.
    /// </summary>
    public interface IWorkerService
    {
        Task ProcessShowYear(MotocycleCreatedEvent? @event);
    }
}
