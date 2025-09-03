namespace MotoManager.Domain.Constants
{
    /// <summary>
    /// Constants for MongoDB configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MongoConstants
    {
        /// <summary>
        /// The name of the MongoDB database.
        /// </summary>
        public static readonly string DatabaseName =
            Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")!;

        /// <summary>
        /// The connection string for MongoDB.
        /// </summary>
        public static readonly string ConnectionString =
            Environment.GetEnvironmentVariable("MONGO_DATABASE_URL")!;
    }

    /// <summary>
    /// Constants for RabbitMQ configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RabbitMqConstants
    {
        /// <summary>
        /// The RabbitMQ host name.
        /// </summary>
        public static readonly string HostName =
            Environment.GetEnvironmentVariable("RABBITMQ_HOST")!;
        /// <summary>
        /// The RabbitMQ port.
        /// </summary>
        public static readonly int Port =
            int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT")!);
        /// <summary>
        /// The RabbitMQ user name.
        /// </summary>
        public static readonly string User =
            Environment.GetEnvironmentVariable("RABBITMQ_USER")!;
        /// <summary>
        /// The RabbitMQ password.
        /// </summary>
        public static readonly string Password =
            Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;
        /// <summary>
        /// The RabbitMQ exchange name.
        /// </summary>
        public static readonly string ExchangeName =
                Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGENAME");
        /// <summary>
        /// The RabbitMQ routing key.
        /// </summary>
        public static readonly string RoutingKey =
                Environment.GetEnvironmentVariable("RABBITMQ_ROUTINGKEY");
    }

    /// <summary>
    /// Constants for MinIO configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MinIoConstants
    {
        /// <summary>
        /// The MinIO endpoint URL.
        /// </summary>
        public static readonly string Endpoint =
            Environment.GetEnvironmentVariable("MINIO_ENDPOINT")!;
        /// <summary>
        /// The MinIO access key.
        /// </summary>
        public static readonly string AccessKey =
            Environment.GetEnvironmentVariable("MINIO_ACCESSKEY")!;
        /// <summary>
        /// The MinIO secret key.
        /// </summary>
        public static readonly string SecretKey =
            Environment.GetEnvironmentVariable("MINIO_SECRETKEY")!;
        /// <summary>
        /// The MinIO bucket name.
        /// </summary>
        public static readonly string BucketName =
            Environment.GetEnvironmentVariable("MINIO_BUCKETNAME")!;
    }

    /// <summary>
    /// Constants for HTTP header configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HeaderConstants
    {
        /// <summary>
        /// The name of the correlation ID header.
        /// </summary>
        public const string CorrelationIdHeader = "X-Correlation-Id";
    }

    /// <summary>
    /// Constants for Redis configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RedisConstants
    {
        /// <summary>
        /// The connection string for Redis.
        /// </summary>
        public static readonly string ConnectionString =
            Environment.GetEnvironmentVariable("REDIS_URL")!;
    }
}