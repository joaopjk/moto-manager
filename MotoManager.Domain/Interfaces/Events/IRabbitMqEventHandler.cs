namespace MotoManager.Domain.Interfaces.Events
{
    /// <summary>
    /// Interface for handling RabbitMQ event publishing and subscription.
    /// </summary>
    public interface IRabbitMqEventHandler
    {
        /// <summary>
        /// Publishes a message to a RabbitMQ exchange with the specified routing key.
        /// </summary>
        /// <typeparam name="T">Type of the message.</typeparam>
        /// <param name="exchange">Exchange name.</param>
        /// <param name="routingKey">Routing key.</param>
        /// <param name="message">Message to publish.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PublishAsync<T>(string exchange, string routingKey, T message);
        /// <summary>
        /// Subscribes to a RabbitMQ queue and processes messages using the provided handler.
        /// </summary>
        /// <typeparam name="T">Type of the message.</typeparam>
        /// <param name="exchange">Exchange name.</param>
        /// <param name="routingKey">Routing key.</param>
        /// <param name="queue">Queue name.</param>
        /// <param name="onMessage">Handler for incoming messages.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SubscribeAsync<T>(string exchange, string routingKey, string queue, Func<T, Task> onMessage, CancellationToken cancellationToken = default);
    }
}
