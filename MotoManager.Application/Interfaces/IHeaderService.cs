namespace MotoManager.Application.Interfaces
{
    /// <summary>
    /// Interface responsible for extracting information from HTTP headers.
    /// </summary>
    public interface IHeaderService
    {
        /// <summary>
        /// Gets the correlation ID from the HTTP request header.
        /// </summary>
        /// <returns>The correlation ID if present; otherwise, null.</returns>
        string GetCorrelationId();

        /// <summary>
        /// Gets the user ID. This is a symbolic value because authentication is not implemented.
        /// </summary>
        /// <returns>Returns string for user ID.</returns>
        public string GetUserId();

        /// <summary>
        /// Gets both the correlation ID and user ID from the HTTP context.
        /// </summary>
        /// <returns>A tuple containing the correlation ID and user ID.</returns>
        public (string correlationId, string userId) GetRequestInfo();
    }
}
