namespace MotoManager.Shared.Security
{
    /// <summary>
    /// Utility class for sanitizing strings by removing non-alphanumeric characters from long inputs.
    /// </summary>
    public static class StringSanitizer
    {
        /// <summary>
        /// Removes non-alphanumeric characters from the input string if its length is greater than 10.
        /// Returns the original input for null, empty, or short strings.
        /// </summary>
        /// <param name="input">The string to sanitize.</param>
        /// <returns>Sanitized string with only letters and digits, trimmed.</returns>
        public static string Sanitize(this string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= 10)
                return input;

            Span<char> buffer = stackalloc char[input.Length];
            var idx = 0;

            foreach (var c in input.Where(char.IsLetterOrDigit))
            {
                buffer[idx++] = c;
            }

            return new string(buffer[..idx]).Trim();
        }
    }
}
