namespace MotoManager.IntegrationTests.Events
{
    /// <summary>
    /// Integration tests for RabbitMqEventHandler using a real RabbitMQ container via TestContainers.
    /// </summary>
    public class RabbitMqEventHandlerIntegrationTests : IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer;
        private RabbitMqEventHandler _eventHandler = null!;
        private const string Exchange = "test-exchange";
        private const string RoutingKey = "test.key";
        private const string Queue = "test-queue";

        public RabbitMqEventHandlerIntegrationTests()
        {
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3.12-management-alpine")
                .WithUsername("guest")
                .WithPassword("guest")
                .WithCleanUp(true)
                .Build();
        }

        /// <summary>
        /// Initializes the RabbitMQ container and event handler before each test.
        /// </summary>
        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
            Environment.SetEnvironmentVariable("RABBITMQ_HOST", _rabbitMqContainer.Hostname);
            Environment.SetEnvironmentVariable("RABBITMQ_PORT", _rabbitMqContainer.GetMappedPublicPort(5672).ToString());
            Environment.SetEnvironmentVariable("RABBITMQ_USER", "guest");
            Environment.SetEnvironmentVariable("RABBITMQ_PASSWORD", "guest");
            _eventHandler = new RabbitMqEventHandler();
        }

        /// <summary>
        /// Disposes the RabbitMQ container after each test.
        /// </summary>
        public async Task DisposeAsync()
        {
            await _rabbitMqContainer.DisposeAsync();
        }

        /// <summary>
        /// Verifies that PublishAsync publishes a message and SubscribeAsync receives it.
        /// </summary>
        [Fact]
        public async Task PublishAndSubscribe_ShouldSendAndReceiveMessage()
        {
            var testMessage = new TestMessage { Id = Guid.NewGuid().ToString(), Content = "Hello RabbitMQ!" };
            var received = false;
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // Start subscriber
            var subscribeTask = _eventHandler.SubscribeAsync<TestMessage>(Exchange, RoutingKey, Queue, msg =>
            {
                Assert.Equal(testMessage.Id, msg.Id);
                Assert.Equal(testMessage.Content, msg.Content);
                received = true;
                cts.Cancel();
                return Task.CompletedTask;
            }, cts.Token);

            // Give time for subscription to be ready
            await Task.Delay(1000);

            // Publish message
            await _eventHandler.PublishAsync(Exchange, RoutingKey, testMessage);

            // Wait for message to be received or timeout
            try
            {
                await subscribeTask;
            }
            catch (OperationCanceledException) { }

            Assert.True(received, "Message was not received by subscriber.");
        }
    }
}
