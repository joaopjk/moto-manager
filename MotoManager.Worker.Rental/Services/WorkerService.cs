using AutoMapper;
using MotoManager.Domain.Entities;
using MotoManager.Domain.Events;
using MotoManager.Domain.Interfaces.Repositories;
using MotoManager.Shared.Logger;

namespace MotoManager.Worker.MotocycleCreate.Services
{
    /// <summary>
    /// Worker service responsible for processing MotocycleCreatedEvent events and registering motorcycles.
    /// </summary>
    public class WorkerService : IWorkerService
    {
        /// <summary>
        /// Repository for motorcycle entities.
        /// </summary>
        private readonly IMotocycleRepository _motocycleRepository;
        /// <summary>
        /// Logger for worker service operations.
        /// </summary>
        private readonly BaseLogger<WorkerService> _logger;
        /// <summary>
        /// AutoMapper instance for mapping event data to entities.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerService"/> class.
        /// </summary>
        /// <param name="motocycleRepository">Repository for motorcycle entities.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        /// <param name="logger">Logger for worker service operations.</param>
        public WorkerService(
            IMotocycleRepository motocycleRepository,
            IMapper mapper,
            BaseLogger<WorkerService> logger)
        {
            _motocycleRepository = motocycleRepository ?? throw new ArgumentException(nameof(motocycleRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        /// <summary>
        /// Processes a MotocycleCreatedEvent, registering the motorcycle if it does not already exist.
        /// </summary>
        /// <param name="event">The MotocycleCreatedEvent to process. Can be null.</param>
        public async Task ProcessMotocycleCreatedEvent(MotocycleCreatedEvent? @event)
        {
            const string methodName = nameof(ProcessMotocycleCreatedEvent);
            try
            {
                if (@event is null || string.IsNullOrWhiteSpace(@event.Identifier))
                {
                    _logger.LogWarning(@event?.CorrelationId, @event?.UserId, methodName, "Received null or invalid event.");
                    return;
                }

                var motocycleExits = await _motocycleRepository.GetMotocycleByPlate(@event.Plate);
                if (motocycleExits is not null)
                {
                    _logger.LogWarning(@event?.CorrelationId, @event?.UserId, methodName, "Received null or invalid event.");
                    return;
                }

                var result = await _motocycleRepository.RegisterMotocycle(_mapper.Map<MotocycleEntity>(@event));
                _logger.LogInformation(@event?.CorrelationId, @event?.UserId, methodName, $"Motocycle {@event.Identifier} registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(@event?.CorrelationId, @event?.UserId, methodName, ex, $"Error processing event for motocycle {@event?.Identifier}: {ex.Message}");
            }
        }
    }
}
