namespace MotoManager.Infrastructure.Events
{
    /// <summary>
    /// Event handler for publishing and subscribing to RabbitMQ events.
    /// </summary>
    public class RabbitMqEventHandler : IRabbitMqEventHandler
    {
        private readonly ConnectionFactory _factory = new()
        {
            HostName = RabbitMqConstants.HostName,
            Port = RabbitMqConstants.Port,
            UserName = RabbitMqConstants.User,
            Password = RabbitMqConstants.Password
        };

        /// <summary>
        /// Publishes a message to a RabbitMQ exchange with the specified routing key.
        /// </summary>
        public async Task PublishAsync<T>(string exchange, string routingKey, T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true);

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true,
                body: body
            );
        }

        /// <summary>
        /// Subscribes to a RabbitMQ queue and processes messages using the provided handler.
        /// </summary>
        public async Task SubscribeAsync<T>(string exchange, string routingKey, string queue, Func<T, Task> onMessage, CancellationToken cancellationToken = default)
        {
            var connection = await _factory.CreateConnectionAsync(cancellationToken);
            var channel = await connection.CreateChannelAsync(null, cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(queue: queue, exchange: exchange, routingKey: routingKey, cancellationToken: cancellationToken);

            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize<T>(body);
                    if (message != null)
                        await onMessage(message);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(queue: queue, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);

            // Mantém o canal aberto até o cancelamento
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                    await Task.Delay(1000, cancellationToken);
            }
            finally
            {
                await channel.CloseAsync();
                await connection.CloseAsync();
            }
        }
    }
}
