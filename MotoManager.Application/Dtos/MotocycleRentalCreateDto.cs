namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object for creating a motorcycle rental, including partner, motorcycle, dates, and plan.
    /// </summary>
    public record MotocycleRentalCreateDto
    {
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
    }
}
