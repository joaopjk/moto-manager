using MotoManager.Domain.Constants;

namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for extracting information from HTTP headers.
    /// </summary>
    public class HeaderService(IHttpContextAccessor httpContextAccessor) : IHeaderService
    {
        /// <summary>
        /// Gets the correlation ID from the HTTP request header.
        /// </summary>
        /// <returns>The correlation ID if present; otherwise, null.</returns>
        public string GetCorrelationId()
        {
            var context = httpContextAccessor.HttpContext;
            if (context != null && context.Request.Headers.TryGetValue(HeaderConstants.CorrelationIdHeader, out var correlationId))
                return correlationId.ToString();

            return null;
        }

        /// <summary>
        /// Gets the user ID. This is a symbolic value because authentication is not implemented.
        /// </summary>
        /// <returns>Returns "9999" as a placeholder for user ID.</returns>
        public string GetUserId()
        {
            return "9999";
        }

        /// <summary>
        /// Gets both the correlation ID and user ID from the HTTP context.
        /// </summary>
        /// <returns>A tuple containing the correlation ID and user ID.</returns>
        public (string correlationId, string userId) GetRequestInfo()
        {
            var correlationId = GetCorrelationId();
            var userId = GetUserId();
            return (correlationId, userId);
        }
    }
}
