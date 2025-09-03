namespace MotoManager.Api.Controllers.V1
{
    /// <summary>
    /// Controller responsible for handling rental operations such as registration and retrieval.
    /// </summary>
    [Route("locacao")]
    [ApiController]
    public class RentalController(IMotocycleRentalService service) : ControllerBase
    {
        private readonly IMotocycleRentalService _service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Registers a new rental.
        /// </summary>
        /// <param name="createDto">DTO containing rental information.</param>
        /// <returns>HTTP 200 if successful, otherwise HTTP 400 with error message.</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterRental([FromBody] MotocycleRentalCreateDto createDto)
        {
            var result = await _service.RegisterRental(createDto);
            if (result.Success)
                return Ok();
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Retrieves a rental by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the rental.</param>
        /// <returns>HTTP 200 with rental DTO if found, otherwise HTTP 400 with error message.</returns>
        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetRental(string identifier)
        {
            var result = await _service.GetMotocycleRentalByIdentifier(identifier);
            if (result.Success)
                return Ok(result.Value);
            return BadRequest(result.Message);
        }
    }
}
