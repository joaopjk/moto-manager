namespace MotoManager.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a vehicle plate with validation for old and Mercosul formats.
    /// </summary>
    public sealed class PlateValueObject
    {
        private static readonly Regex OldPattern = new(@"^[A-Z]{3}-?[0-9]{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex MercosulPattern = new(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the validated plate value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PlateValueObject"/> and validates the plate value.
        /// </summary>
        /// <param name="value">Plate value to validate.</param>
        public PlateValueObject(string value)
        {
            value = value?.ToUpperInvariant().Trim() ?? string.Empty;

            if (!IsValid(value))
                return;

            Value = value;
        }

        /// <summary>
        /// Validates a plate value for old and Mercosul formats.
        /// </summary>
        /// <param name="value">Plate value to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.ToUpperInvariant().Trim();

            return OldPattern.IsMatch(value) || MercosulPattern.IsMatch(value);
        }
    }
}
