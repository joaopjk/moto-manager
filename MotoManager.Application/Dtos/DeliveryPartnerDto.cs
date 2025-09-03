namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object representing a delivery partner with personal and license information.
    /// </summary>
    public class DeliveryPartnerDto
    {
        /// <summary>
        /// Unique identifier for the delivery partner.
        /// </summary>
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        /// <summary>
        /// Name of the delivery partner.
        /// </summary>
        [JsonPropertyName("nome")]
        public string Name { get; set; }

        /// <summary>
        /// CNPJ (Cadastro Nacional da Pessoa Jurídica) of the delivery partner.
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Date of birth of the delivery partner.
        /// </summary>
        [JsonPropertyName("data_nascimento")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Driver license number (CNH) of the delivery partner.
        /// </summary>
        [JsonPropertyName("numero_cnh")]
        public string DriverLicenseNumber { get; set; }

        /// <summary>
        /// Driver license type (CNH type) of the delivery partner.
        /// </summary>
        [JsonPropertyName("tipo_cnh")]
        public string DriverLicenseType { get; set; }

        /// <summary>
        /// Base64 string of the driver's license image.
        /// </summary>
        [JsonPropertyName("imagem_cnh")]
        public string ImageDriverLicenseNumber { get; set; }
    }
}
