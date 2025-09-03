using System.Diagnostics.CodeAnalysis;
using MotoManager.Domain.Constants;
using MotoManager.Domain.Events;
using MotoManager.Domain.Interfaces.Events;
using MotoManager.Worker.MotocycleCreate.Services;

namespace MotoManager.Worker.MotocycleCreate
{
    [ExcludeFromCodeCoverage]
    public class Worker(
        IWorkerService workerService,
        IRabbitMqEventHandler rabbitConsumer) : BackgroundService
    {
        private readonly IWorkerService _workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
        private readonly IRabbitMqEventHandler _rabbitConsumer = rabbitConsumer ?? throw new ArgumentNullException(nameof(rabbitConsumer));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _rabbitConsumer.SubscribeAsync<MotocycleCreatedEvent>(
                    RabbitMqConstants.ExchangeName,
                    RabbitMqConstants.RoutingKey,
                    queue: "rental-created-queue",
                    onMessage: async (msg) => { await _workerService.ProcessMotocycleCreatedEvent(msg); },
                    cancellationToken: stoppingToken
                );
            }
        }
    }
}
