namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object representing a motorcycle rental with details about the rental and pricing.
    /// </summary>
    public record MotocycleRentalDto
    {
        /// <summary>
        /// Unique identifier for the rental.
        /// </summary>
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        /// <summary>
        /// Identifier of the delivery partner.
        /// </summary>
        [JsonPropertyName("entregador_id")]
        public string IdDeliveryPartner { get; set; }

        /// <summary>
        /// Identifier of the motorcycle.
        /// </summary>
        [JsonPropertyName("moto_id")]
        public string IdMotocycle { get; set; }

        /// <summary>
        /// Start date of the rental.
        /// </summary>
        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the rental.
        /// </summary>
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Expected end date of the rental.
        /// </summary>
        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        /// <summary>
        /// Rental plan identifier.
        /// </summary>
        [JsonPropertyName("plano")]
        public int Plan { get; set; }

        /// <summary>
        /// Daily rent value for the rental.
        /// </summary>
        [JsonPropertyName("valor_diaria")]
        public decimal DailyRent { get; set; }
    }
}
