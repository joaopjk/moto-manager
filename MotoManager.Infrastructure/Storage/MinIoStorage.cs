namespace MotoManager.Infrastructure.Storage
{
    /// <summary>
    /// Implementation of IMinIoStorage for handling file operations with MinIO.
    /// </summary>
    public class MinIoStorage(string endpoint, string accessKey, string secretKey, string bucketName) : IMinIoStorage
    {
        /// <summary>
        /// MinIO client used for storage operations.
        /// </summary>
        private readonly IMinioClient _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();

        /// <summary>
        /// Uploads a file asynchronously to MinIO storage, creating the bucket if it does not exist.
        /// </summary>
        /// <param name="objectName">Name of the object to store.</param>
        /// <param name="data">Stream containing the file data.</param>
        /// <param name="contentType">Content type of the file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UploadFileAsync(string objectName, Stream data, string contentType)
        {
            await CreateBucketIfNotExistsAsync();
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithContentType(contentType)
                .WithObjectSize(data.Length));
        }

        /// <summary>
        /// Checks if the bucket exists in MinIO storage.
        /// </summary>
        /// <returns>True if the bucket exists; otherwise, false.</returns>
        private async Task<bool> BucketExistsAsync()
        {
            return await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));

        }

        /// <summary>
        /// Creates the bucket in MinIO storage if it does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateBucketIfNotExistsAsync()
        {
            if (!await BucketExistsAsync())
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }
    }
}
