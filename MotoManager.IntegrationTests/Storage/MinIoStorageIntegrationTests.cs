namespace MotoManager.IntegrationTests.Storage
{
    /// <summary>
    /// Integration tests for MinIoStorage using a real MinIO container via TestContainers.
    /// </summary>
    public class MinIoStorageIntegrationTests : IAsyncLifetime
    {
        private readonly MinioContainer _minioContainer;
        private MinIoStorage _storage = null!;
        private const string BucketName = "test-bucket";
        private const string AccessKey = "minioadmin";
        private const string SecretKey = "minioadmin";

        public MinIoStorageIntegrationTests()
        {
            _minioContainer = new MinioBuilder()
                .WithImage("minio/minio:latest")
                .WithUsername(AccessKey)
                .WithPassword(SecretKey)
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _minioContainer.StartAsync();
            var endpoint = $"{_minioContainer.Hostname}:{_minioContainer.GetMappedPublicPort(9000)}";
            _storage = new MinIoStorage(endpoint, AccessKey, SecretKey, BucketName);
        }

        public async Task DisposeAsync()
        {
            await _minioContainer.DisposeAsync();
        }

        [Fact]
        public async Task UploadFileAsync_ShouldUploadAndExistInBucket()
        {
            var objectName = "test.txt";
            var content = "Hello MinIO!";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await _storage.UploadFileAsync(objectName, stream, "text/plain");

            // Verifica se o objeto existe no bucket
            var minioClient = new MinioClient()
                .WithEndpoint(_minioContainer.Hostname, _minioContainer.GetMappedPublicPort(9000))
                .WithCredentials(AccessKey, SecretKey)
                .Build();
            var stat = await minioClient.StatObjectAsync(new StatObjectArgs().WithBucket(BucketName).WithObject(objectName));
            Assert.NotNull(stat);
            Assert.Equal(objectName, stat.ObjectName);
        }
    }
}
