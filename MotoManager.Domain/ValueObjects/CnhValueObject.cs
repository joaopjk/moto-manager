namespace MotoManager.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a CNH (Carteira Nacional de Habilitação) number with validation logic.
    /// </summary>
    public sealed class CnhValueObject
    {
        private static readonly Regex CnhRegex = new(@"^\d{11}$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the validated CNH value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CnhValueObject"/> and validates the CNH value.
        /// Throws <see cref="ArgumentException"/> if the value is invalid.
        /// </summary>
        /// <param name="value">CNH number to validate.</param>
        public CnhValueObject(string value)
        {
            value = value?.Trim() ?? string.Empty;

            if (!IsValid(value))
                throw new ArgumentException("Invalid CNH.", nameof(value));

            Value = value;
        }

        /// <summary>
        /// Validates a CNH number according to format and check digit rules.
        /// </summary>
        /// <param name="value">CNH number to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();

            if (!CnhRegex.IsMatch(value))
                return false;

            if (new string(value[0], value.Length) == value)
                return false;

            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += int.Parse(value[i].ToString()) * (9 - i);

            var dv1 = sum % 11;
            if (dv1 >= 10) dv1 = 0;

            sum = 0;
            for (var i = 0; i < 9; i++)
                sum += int.Parse(value[i].ToString()) * (i + 1);

            var dv2 = sum % 11;
            if (dv2 >= 10) dv2 = 0;

            var checkDigits = value.Substring(9, 2);
            var calculatedDigits = $"{dv1}{dv2}";

            return checkDigits == calculatedDigits;
        }
    }
}
