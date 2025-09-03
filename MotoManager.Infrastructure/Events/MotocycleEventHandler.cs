namespace MotoManager.Infrastructure.Events
{
    [ExcludeFromCodeCoverage]
    public class MotocycleEventHandler(IRabbitMqEventHandler rabbitMqEventHandler) : IMotocycleEventHandler
    {
        private readonly IRabbitMqEventHandler _rabbitMqEventHandler = rabbitMqEventHandler ?? throw new ArgumentNullException(nameof(rabbitMqEventHandler));

        /// <summary>
        /// Publishes a motorcycle created event to RabbitMQ.
        /// </summary>
        public async Task<bool> PublishMotocycleEvent(MotocycleCreatedEvent @event)
        {
            await _rabbitMqEventHandler.PublishAsync(
                RabbitMqConstants.ExchangeName,
                RabbitMqConstants.RoutingKey,
                @event);

            return true;
        }
    }
}
