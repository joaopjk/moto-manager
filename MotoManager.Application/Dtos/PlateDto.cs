namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object representing a vehicle plate for update operations.
    /// </summary>
    public record PlateDto
    {
        /// <summary>
        /// Plate of the vehicle.
        /// </summary>
        [JsonPropertyName("placa")]
        public string Plate { get; set; }
    }
}
