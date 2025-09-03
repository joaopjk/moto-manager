namespace MotoManager.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a CNPJ (Cadastro Nacional da Pessoa Jurídica) number with validation logic.
    /// </summary>
    public sealed class CnpjValueObject
    {
        private static readonly Regex CnpjRegex = new(@"^\d{14}$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the validated CNPJ value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CnpjValueObject"/> and validates the CNPJ value.
        /// </summary>
        /// <param name="value">CNPJ number to validate.</param>
        public CnpjValueObject(string value)
        {
            value = value?.Trim() ?? string.Empty;

            if (!IsValid(value))
                return;

            Value = value;
        }

        /// <summary>
        /// Validates a CNPJ number according to format and check digit rules.
        /// </summary>
        /// <param name="value">CNPJ number to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();

            if (!CnpjRegex.IsMatch(value))
                return false;

            if (new string(value[0], value.Length) == value)
                return false;

            int[] multiplier1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            var tempCnpj = value[..12];
            var sum = 0;

            for (var i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += digit1;
            sum = 0;

            for (var i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            var checkDigits = value.Substring(12, 2);
            var calculatedDigits = $"{digit1}{digit2}";

            return checkDigits == calculatedDigits;
        }
    }
}
