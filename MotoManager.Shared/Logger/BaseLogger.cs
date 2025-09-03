namespace MotoManager.Shared.Logger;

[ExcludeFromCodeCoverage]
public class BaseLogger<T>(ILogger<T> logger)
{
    private readonly ILogger<T> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private static string FormatMessage(string correlationId, string userId, string methodName, string message)
    {
        return DataMasking.Mask($"[CorrelationId: {correlationId}] [UserId: {userId}] [Method: {methodName}] - Message: {message}");
    }

    public void LogInformation(string correlationId, string userId, string methodName, string message, params object[] args)
    {
        if (!string.IsNullOrWhiteSpace(message))
            _logger.LogInformation(FormatMessage(correlationId, userId, methodName, message), args);
    }

    public void LogWarning(string correlationId, string userId, string methodName, string message, params object[] args)
    {
        if (!string.IsNullOrWhiteSpace(message))
            _logger.LogWarning(FormatMessage(correlationId, userId, methodName, message), args);
    }

    public void LogError(string correlationId, string userId, string methodName, Exception ex, string message = null, params object[] args)
    {
        if (ex == null) return;
        if (!string.IsNullOrWhiteSpace(message))
            _logger.LogError(ex, FormatMessage(correlationId, userId, methodName, message), args);
        else
            _logger.LogError(ex, FormatMessage(correlationId, userId, methodName, ex.Message));
    }
}