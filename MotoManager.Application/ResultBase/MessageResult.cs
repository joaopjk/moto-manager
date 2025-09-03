namespace MotoManager.Application.ResultBase
{
    /// <summary>
    /// Represents a message result containing a message string.
    /// </summary>
    public record MessageResult(string Message)
    {
        /// <summary>
        /// Message string describing the result.
        /// </summary>
        [JsonPropertyName("mensagem")]
        public string Message { get; set; } = Message;
    }
}
