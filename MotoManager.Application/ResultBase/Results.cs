namespace MotoManager.Application.ResultBase
{
    /// <summary>
    /// Represents the result of an operation, including success status, error message, value, and a message result.
    /// </summary>
    public record Result<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// Error message if the operation failed.
        /// </summary>
        public string Error { get; }
        /// <summary>
        /// Value returned by the operation.
        /// </summary>
        public T Value { get; }
        /// <summary>
        /// Message result containing additional information about the operation.
        /// </summary>
        public MessageResult Message { get; }
        private Result(T value, bool success, string error, string message)
        {
            Value = value;
            Success = success;
            Error = error;
            Message = new MessageResult(message);
        }

        public static Result<T> Ok(T value, string message) => new(value, true, null!, message);
        public static Result<T> Ok() => new(default, true, null!, null);
        public static Result<T> Ok(T value) => new(value, true, null!, null);
        public static Result<T> Ok(string message) => new(default, true, null!, message);
        public static Result<T> Fail(string message) => new(default!, false, null, message);
    }
}
