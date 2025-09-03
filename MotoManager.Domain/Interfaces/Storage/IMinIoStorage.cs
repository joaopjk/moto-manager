namespace MotoManager.Domain.Interfaces.Storage
{
    /// <summary>
    /// Interface for MinIO storage operations.
    /// </summary>
    public interface IMinIoStorage
    {
        /// <summary>
        /// Uploads a file asynchronously to MinIO storage.
        /// </summary>
        /// <param name="objectName">Name of the object to store.</param>
        /// <param name="data">Stream containing the file data.</param>
        /// <param name="contentType">Content type of the file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UploadFileAsync(string objectName, Stream data, string contentType);
    }
}
