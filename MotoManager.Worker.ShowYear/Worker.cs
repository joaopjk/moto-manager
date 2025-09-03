using MotoManager.Domain.Constants;
using MotoManager.Domain.Events;
using MotoManager.Domain.Interfaces.Events;
using MotoManager.Worker.ShowYear.Services;
using System.Diagnostics.CodeAnalysis;

namespace MotoManager.Worker.ShowYear
{
    [ExcludeFromCodeCoverage]
    public class Worker(
        IRabbitMqEventHandler rabbitConsumer,
        IWorkerService workerService) : BackgroundService
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
                    queue: "rental-show-year-queue",
                    onMessage: async (msg) => { await _workerService.ProcessShowYear(msg); },
                    cancellationToken: stoppingToken
                );
            }
        }
    }
}
