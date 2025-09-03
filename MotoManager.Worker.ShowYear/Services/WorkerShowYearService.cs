using MotoManager.Domain.Events;
using MotoManager.Shared.Logger;

namespace MotoManager.Worker.ShowYear.Services
{
    /// <summary>
    /// Worker service responsible for processing MotocycleCreatedEvent events and logging information about the motorcycle's year.
    /// </summary>
    public class WorkerShowYearService : IWorkerService
    {
        /// <summary>
        /// Logger for worker service operations.
        /// </summary>
        private readonly BaseLogger<WorkerShowYearService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerShowYearService"/> class.
        /// </summary>
        /// <param name="logger">Logger for worker service operations.</param>
        public WorkerShowYearService(BaseLogger<WorkerShowYearService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Processes a MotocycleCreatedEvent, logging information if the motorcycle is from the year 2024.
        /// </summary>
        /// <param name="event">The MotocycleCreatedEvent to process. Can be null.</param>
        public Task ProcessShowYear(MotocycleCreatedEvent? @event)
        {
            const string methodName = nameof(ProcessShowYear);
            try
            {
                if (@event is null || string.IsNullOrWhiteSpace(@event.Identifier))
                {
                    _logger.LogWarning(@event?.CorrelationId, @event?.UserId, methodName, "Received null or invalid event.");
                }
                else if (@event.Year == 2024)
                    _logger.LogInformation(@event.CorrelationId, @event.UserId, methodName, $"Motocycle {@event.Identifier} is from the year 2024.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(@event?.CorrelationId, @event?.UserId, methodName, ex, $"Error processing event for motocycle {@event?.Identifier}: {ex.Message}");
                return Task.CompletedTask;
            }
        }
    }
}
