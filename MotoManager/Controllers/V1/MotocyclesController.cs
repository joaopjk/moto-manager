namespace MotoManager.Api.Controllers.V1
{
    /// <summary>
    /// Controller responsible for handling motorcycle operations such as registration, retrieval, search, update, and deletion.
    /// </summary>
    [Route("motos")]
    [ApiController]
    public class MotocyclesController(IMotocycleService motocycleService) : ControllerBase
    {
        /// <summary>
        /// Registers a new motorcycle.
        /// </summary>
        /// <param name="dto">DTO containing motorcycle information.</param>
        /// <returns>HTTP 201 if successful, otherwise HTTP 400 with error message.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterMotocycle([FromBody] MotocycleDto dto)
        {
            var result = await motocycleService.RegisterMotocycle(dto);
            if (result.Success)
                return Created();
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Retrieves a motorcycle by its identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the motorcycle.</param>
        /// <returns>HTTP 200 with motorcycle DTO if found, otherwise HTTP 400 with error message.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMotocycleByIdentifier([FromRoute] string id)
        {
            var result = await motocycleService.GetMotocycleByIdentifier(id);
            if (result.Success)
                return Ok(result.Value);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Searches for motorcycles by plate. If plate is empty, returns all motorcycles.
        /// </summary>
        /// <param name="placa">Plate to search for. If null or empty, returns all motorcycles.</param>
        /// <returns>HTTP 200 with list of motorcycles if found, otherwise HTTP 400 with error message.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMotocycle([FromQuery] string? placa)
        {
            var result = await motocycleService.SearchMotocycle(placa);
            if (result.Success)
                return Ok(result.Value);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Updates the plate of a motorcycle identified by its identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the motorcycle.</param>
        /// <param name="dto">DTO containing the new plate information.</param>
        /// <returns>HTTP 200 if successful, otherwise HTTP 400 with error message.</returns>
        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdatePlateMotocycle([FromRoute] string id, [FromBody] PlateDto dto)
        {
            var result = await motocycleService.UpdatePlateMotocycle(id, dto);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Deletes a motorcycle by its identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the motorcycle.</param>
        /// <returns>HTTP 200 if successful, otherwise HTTP 400 with error message.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotocycle([FromRoute] string id)
        {
            var result = await motocycleService.DeleteMotocycle(id);
            if (result.Success)
                return Ok();
            return BadRequest(result.Message);
        }
    }
}
