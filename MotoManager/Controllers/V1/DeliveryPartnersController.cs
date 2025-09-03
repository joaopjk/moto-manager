namespace MotoManager.Api.Controllers.V1;

/// <summary>
/// Controller responsible for handling delivery partner operations such as registration and CNH image upload.
/// </summary>
[Route("entregadores")]
[ApiController]
public class DeliveryPartnersController(IDeliveryPartnerService deliveryPartnerService) : ControllerBase
{
    private readonly IDeliveryPartnerService _deliveryPartnerService = deliveryPartnerService ?? throw new ArgumentNullException(nameof(deliveryPartnerService));

    /// <summary>
    /// Registers a new delivery rider.
    /// </summary>
    /// <param name="dto">DTO containing delivery partner information.</param>
    /// <returns>HTTP 200 if successful, otherwise HTTP 400 with error message.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDeliveryRider([FromBody] DeliveryPartnerDto dto)
    {
        var result = await _deliveryPartnerService.RegisterDeliveryRider(dto);
        if (result.Success)
            return Ok();
        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uploads the CNH image for a delivery partner.
    /// </summary>
    /// <param name="id">Identifier of the delivery partner.</param>
    /// <param name="file">DTO containing the base64 image string.</param>
    /// <returns>HTTP 200 if successful, otherwise HTTP 400 with error message.</returns>
    [HttpPost("{id}/cnh")]
    public async Task<IActionResult> UploadCnhImage([FromRoute] string id, [FromBody] ImageDriverLicenseNumberDto file)
    {
        var result = await _deliveryPartnerService.UploadDriverLicenseImage(id, file.ImageDriverLicenseNumber);
        if (result.Success)
            return Ok();
        return BadRequest(result.Message);
    }
}