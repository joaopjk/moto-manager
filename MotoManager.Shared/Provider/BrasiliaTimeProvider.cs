namespace MotoManager.Shared.Provider
{
    /// <summary>
    /// Fornece o horário atual de Brasília, independente do sistema operacional.
    /// </summary>
    public static class BrasiliaTimeProvider
    {
        /// <summary>
        /// Representa o fuso horário de Brasília, compatível com Windows e Linux.
        /// </summary>
        private static readonly TimeZoneInfo BrasiliaTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows() ? "E. South America Standard Time" : "America/Sao_Paulo");

        /// <summary>
        /// Obtém o horário atual de Brasília.
        /// </summary>
        public static DateTime Now =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrasiliaTimeZone);
    }
}
