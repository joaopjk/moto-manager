namespace MotoManager.Domain.Enums.Parses
{
    /// <summary>
    /// Provides parsing logic for CNH (driver license type) strings to LicenseDriverType enum values.
    /// </summary>
    public static class ParseCnhType
    {
        /// <summary>
        /// Parses a CNH type string and returns the corresponding LicenseDriverType enum value.
        /// </summary>
        /// <param name="tipoCnh">The CNH type string to parse (e.g., "A", "B", "A+B").</param>
        /// <returns>The corresponding LicenseDriverType value, or LicenseDriverType.Invalid if not recognized.</returns>
        public static LicenseDriverType Parse(string tipoCnh)
        {
            return tipoCnh.Trim().ToUpperInvariant() switch
            {
                "A" => LicenseDriverType.A,
                "B" => LicenseDriverType.B,
                "A+B" => LicenseDriverType.AB,
                _ => LicenseDriverType.Invalid
            };
        }
    }
}
