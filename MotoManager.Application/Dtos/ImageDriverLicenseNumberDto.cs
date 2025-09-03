namespace MotoManager.Application.Dtos
{
    /// <summary>
    /// Data transfer object for uploading a driver's license image in base64 format.
    /// </summary>
    public record ImageDriverLicenseNumberDto
    {
        /// <summary>
        /// Base64 string of the driver's license image.
        /// </summary>
        [JsonPropertyName("imagem_cnh")]
        public string ImageDriverLicenseNumber { get; set; }
    }
}
