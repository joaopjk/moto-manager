namespace MotoManager.Domain.Interfaces.Services
{
    /// <summary>
    /// Service responsible for validating and converting base64 images .
    /// </summary>
    public interface IImageValidatorService
    {
        /// <summary>
        /// Converts a base64 image string to a MemoryStream, validates its size and mime type, and generates a file name.
        /// </summary>
        /// <param name="identifier">Identifier for the delivery partner.</param>
        /// <param name="base64Image">Base64 string of the image.</param>
        /// <returns>Tuple containing the image stream, file name, and content type.</returns>
        (MemoryStream stream, string fileName, string contentType) ConvertBase64ImageToStream(string identifier, string base64Image);
    }
}
