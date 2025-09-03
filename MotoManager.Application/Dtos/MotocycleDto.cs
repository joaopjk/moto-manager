namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object representing a motorcycle with identifier, year, model, and plate.
    /// </summary>
    public class MotocycleDto
    {
        /// <summary>
        /// Unique identifier for the motorcycle.
        /// </summary>
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        /// <summary>
        /// Year of the motorcycle.
        /// </summary>
        [JsonPropertyName("ano")]
        public int Year { get; set; }

        /// <summary>
        /// Model of the motorcycle.
        /// </summary>
        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        /// <summary>
        /// Plate of the motorcycle.
        /// </summary>
        [JsonPropertyName("placa")]
        public string Plate { get; set; }
    }
}