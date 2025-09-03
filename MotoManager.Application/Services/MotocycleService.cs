using MotoManager.Domain.AggregateRoots;
using MotoManager.Shared.Provider;

namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for motorcycle operations such as registration, retrieval, search, update, and deletion.
    /// </summary>
    public class MotocycleService(
        IMotocycleRepository motocycleRepository,
        IMotocycleEventHandler motocycleEventHandler,
        IMotocycleRentalRepository motocycleRentalRepository,
        IHeaderService headerService,
        IMapper mapper,
        BaseLogger<MotocycleService> logger) : IMotocycleService
    {
        private readonly IMotocycleRepository _motocycleRepository = motocycleRepository ?? throw new ArgumentException(nameof(motocycleRepository));
        private readonly IMotocycleEventHandler _motocycleEventHandler = motocycleEventHandler ?? throw new ArgumentException(nameof(motocycleEventHandler));
        private readonly IMotocycleRentalRepository _motocycleRentalRepository = motocycleRentalRepository ?? throw new ArgumentException(nameof(motocycleRentalRepository));
        private readonly IHeaderService _headerService = headerService ?? throw new ArgumentException(nameof(headerService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        private readonly BaseLogger<MotocycleService> _logger = logger ?? throw new ArgumentException(nameof(logger));

        /// <summary>
        /// Registers a new motorcycle after validating the provided data and business rules.
        /// </summary>
        /// <param name="dto">DTO containing motorcycle information.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        public async Task<Result<string>> RegisterMotocycle(MotocycleDto dto)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(RegisterMotocycle);

            if (!IsValidMotocycleDto(dto))
            {
                _logger.LogInformation(
                   correlationId,
                   userId,
                   methodName,
                   $"Invalid data for motorcycle registration. DTO: {@dto}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var existingMoto = await _motocycleRepository.GetMotocycleByPlate(dto.Plate);
            if (existingMoto is not null)
            {
                _logger.LogInformation(
                   correlationId,
                   userId,
                   methodName,
                   $"Motorcycle already exists for plate: {dto.Plate}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            await _motocycleEventHandler.PublishMotocycleEvent(
                MotocycleCreatedEventAggregateRoot.CreateCreatedEvent(
                    correlationId,
                    userId,
                    dto.Identifier.Sanitize(),
                    dto.Year,
                    dto.Model.Sanitize(),
                    dto.Plate)
                );

            _logger.LogInformation(
               correlationId,
               userId,
               methodName,
               $"Motorcycle successfully registered. Plate: {dto.Plate}");

            return Result<string>.Ok(null, Resource.MOTOCYCLE_REGISTERED_SUCCESSFULLY);
        }

        /// <summary>
        /// Retrieves a motorcycle entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>Result containing the motorcycle DTO if found, or failure if not found.</returns>
        public async Task<Result<MotocycleDto>> GetMotocycleByIdentifier(string identifier)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(GetMotocycleByIdentifier);

            if (string.IsNullOrEmpty(identifier))
            {
                _logger.LogInformation(
                    correlationId,
                    userId,
                    methodName,
                    "Identifier is null or empty.");
                return Result<MotocycleDto>.Fail(Resource.INVALID_REQUEST_MESSAGE);
            }

            var motocycle = await _motocycleRepository.GetMotocycleByIdentifier(identifier.Trim());
            if (motocycle is null)
            {
                _logger.LogInformation(
                    correlationId,
                    userId,
                    methodName,
                    $"No motorcycle found for identifier: {identifier}");
                return Result<MotocycleDto>.Fail(Resource.MOTOCYCLE_NOT_FOUND);
            }

            _logger.LogInformation(
                correlationId,
                userId,
                methodName,
                $"Motorcycle found for identifier: {identifier}");

            return Result<MotocycleDto>.Ok(_mapper.Map<MotocycleDto>(motocycle));
        }

        /// <summary>
        /// Searches for motorcycles by plate. If plate is empty, returns all motorcycles.
        /// </summary>
        /// <param name="plate">Plate to search for. If null or empty, returns all motorcycles.</param>
        /// <returns>Result containing a list of motorcycle DTOs.</returns>
        public async Task<Result<IEnumerable<MotocycleDto>>> SearchMotocycle(string plate)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(SearchMotocycle);
            IEnumerable<MotocycleEntity> listMotocycle;

            if (string.IsNullOrWhiteSpace(plate))
                listMotocycle = await _motocycleRepository.GetAllMotocycle();
            else
            {
                var moto = await _motocycleRepository.GetMotocycleByPlate(plate.Trim().ToUpperInvariant());
                listMotocycle = moto is not null ? new[] { moto } : Array.Empty<MotocycleEntity>();
            }

            _logger.LogInformation(
                correlationId,
                userId,
                methodName,
                $"Motorcycle(s) found: {listMotocycle.Count()} {(string.IsNullOrEmpty(plate) ? "" : $"Plate: {plate}")}");

            return Result<IEnumerable<MotocycleDto>>.Ok(_mapper.Map<IEnumerable<MotocycleDto>>(listMotocycle));
        }

        /// <summary>
        /// Updates the plate of a motorcycle identified by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <param name="dto">DTO containing the new plate information.</param>
        /// <returns>Result indicating success or failure of the update.</returns>
        public async Task<Result<string>> UpdatePlateMotocycle(string identifier, PlateDto dto)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(UpdatePlateMotocycle);

            if (!IsValidatePlateDto(identifier, dto))
            {
                _logger.LogInformation(
                    correlationId,
                    userId,
                    methodName,
                    $"Invalid data for plate update. Identifier: {identifier}, Plate: {dto?.Plate}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var motocycle = await _motocycleRepository.GetMotocycleByIdentifier(identifier.Trim());
            if (motocycle is null)
            {
                _logger.LogInformation(
                    correlationId,
                    userId,
                    methodName,
                    $"Motorcycle not found for identifier: {identifier}");
                return Result<string>.Fail(Resource.MOTOCYCLE_NOT_FOUND);
            }

            motocycle.Plate = dto.Plate.Trim();

            await _motocycleRepository.UpdateMotocyclePlate(motocycle);
            _logger.LogInformation(
                correlationId,
                userId,
                methodName,
                $"Plate updated successfully for identifier: {identifier}, New Plate: {dto.Plate}");
            return Result<string>.Ok(Resource.PLATE_UPDATED_SUCCESSFULLY);
        }

        /// <summary>
        /// Deletes a motorcycle by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <returns>Result indicating success or failure of the deletion.</returns>
        public async Task<Result<string>> DeleteMotocycle(string identifier)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(DeleteMotocycle);

            if (string.IsNullOrWhiteSpace(identifier))
            {
                _logger.LogWarning(
                    correlationId,
                    userId,
                    methodName,
                    "Identifier is null or empty.");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var motocycle = await _motocycleRepository.GetMotocycleByIdentifier(identifier.Trim());
            if (motocycle is null)
            {
                _logger.LogInformation(
                    correlationId,
                    userId,
                    methodName,
                    $"Motorcycle not found for identifier: {identifier}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var locationExists = await _motocycleRentalRepository.GetMotocycleRentalByMotocycleId(motocycle.Identifier);
            if (locationExists is not null)
            {
                if (locationExists.ExpectedEndDate >= BrasiliaTimeProvider.Now)
                {
                    _logger.LogInformation(
                        correlationId,
                        userId,
                        methodName,
                        $"Cannot delete motorcycle currently rented. Identifier: {identifier}");
                    return Result<string>.Fail(Resource.INVALID_DATA);
                }
            }

            await _motocycleRepository.DeleteMotocycle(identifier.Trim());
            _logger.LogInformation(
                correlationId,
                userId,
                methodName,
                $"Motorcycle removed successfully. Identifier: {identifier}");
            return Result<string>.Ok(Resource.MOTORCYCLE_REMOVED_SUCCESSFULLY);
        }

        /// <summary>
        /// Validates the plate DTO for required fields and business rules.
        /// </summary>
        /// <param name="identifier">Unique identifier of the motorcycle.</param>
        /// <param name="dto">DTO containing plate information.</param>
        /// <returns>True if the DTO is valid; otherwise, false.</returns>
        private static bool IsValidatePlateDto(string identifier, PlateDto dto)
        {
            var validPlate = new PlateValueObject(dto?.Plate);

            return dto != null &&
                   !string.IsNullOrWhiteSpace(identifier) &&
                   !string.IsNullOrWhiteSpace(dto.Plate) &&
                   validPlate.Value != null;
        }

        /// <summary>
        /// Validates the motorcycle DTO for required fields and business rules.
        /// </summary>
        /// <param name="dto">DTO containing motorcycle information.</param>
        /// <returns>True if the DTO is valid; otherwise, false.</returns>
        private static bool IsValidMotocycleDto(MotocycleDto dto)
        {
            var validPlate = new PlateValueObject(dto?.Plate);

            return
                dto != null &&
                !string.IsNullOrWhiteSpace(dto.Identifier) &&
                !string.IsNullOrWhiteSpace(dto.Model) &&
                !string.IsNullOrWhiteSpace(dto.Plate) &&
                validPlate.Value != null;
        }
    }
}
