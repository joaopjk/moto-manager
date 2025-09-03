namespace MotoManager.Domain.Enums
{
    /// <summary>
    /// Represents the types of driver licenses (CNH) available for delivery partners.
    /// </summary>
    public enum LicenseDriverType
    {
        /// <summary>
        /// License type A (motorcycles).
        /// </summary>
        A,
        /// <summary>
        /// License type B (cars).
        /// </summary>
        B,
        /// <summary>
        /// License type AB (motorcycles and cars).
        /// </summary>
        AB,
        /// <summary>
        /// Invalid or unrecognized license type.
        /// </summary>
        Invalid
    }
}
