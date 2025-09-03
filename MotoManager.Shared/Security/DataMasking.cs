namespace MotoManager.Shared.Security
{
    /// <summary>
    /// Utility class for masking sensitive data such as CNPJ and CNH numbers in strings.
    /// </summary>
    public class DataMasking
    {
        /// <summary>
        /// Masks CNPJ and CNH numbers in the input string, replacing leading digits with asterisks.
        /// </summary>
        /// <param name="texto">Input string possibly containing sensitive data.</param>
        /// <returns>Masked string with sensitive numbers obfuscated.</returns>
        public static string Mask(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            var sb = new System.Text.StringBuilder(texto.Length);
            var i = 0;
            while (i < texto.Length)
            {
                // Tenta CNPJ (14 dígitos)
                if (i + 14 <= texto.Length && IsDigits(texto, i, 14))
                {
                    sb.Append(new string('*', 10));
                    sb.Append(texto.Substring(i + 10, 4));
                    i += 14;
                    continue;
                }
                // Tenta CNH (11 dígitos)
                if (i + 11 <= texto.Length && IsDigits(texto, i, 11))
                {
                    sb.Append(new string('*', 7));
                    sb.Append(texto.Substring(i + 7, 4));
                    i += 11;
                    continue;
                }
                // Copia caractere normal
                sb.Append(texto[i]);
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Checks if a substring consists only of digits.
        /// </summary>
        /// <param name="texto">Input string.</param>
        /// <param name="start">Start index of the substring.</param>
        /// <param name="length">Length of the substring.</param>
        /// <returns>True if all characters are digits; otherwise, false.</returns>
        private static bool IsDigits(string texto, int start, int length)
        {
            for (var j = start; j < start + length; j++)
            {
                if (!char.IsDigit(texto[j]))
                    return false;
            }
            return true;
        }
    }
}
