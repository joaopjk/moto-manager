namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for handling motorcycle rental operations, including registration and retrieval.
    /// </summary>
    public class MotocycleRentalService(
        IMotocycleRepository motocycleRepository,
        IRentalPlanService rentalPlanService,
        IHeaderService headerService,
        IDeliveryPartnerRepository deliveryPartnerRepository,
        IMotocycleRentalRepository motocycleRentalRepository,
        IMapper mapper,
        BaseLogger<MotocycleRentalService> logger) : IMotocycleRentalService
    {
        private readonly IMotocycleRepository _motocycleRepository = motocycleRepository ?? throw new ArgumentException(nameof(motocycleRepository));
        private readonly IDeliveryPartnerRepository _deliveryPartnerRepository = deliveryPartnerRepository ?? throw new ArgumentException(nameof(deliveryPartnerRepository));
        private readonly IRentalPlanService _rentalPlanService = rentalPlanService ?? throw new ArgumentException(nameof(rentalPlanService));
        private readonly IMotocycleRentalRepository _motocycleRentalRepository = motocycleRentalRepository ?? throw new ArgumentException(nameof(motocycleRentalRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        private readonly IHeaderService _headerService = headerService ?? throw new ArgumentException(nameof(headerService));
        private readonly BaseLogger<MotocycleRentalService> _logger = logger ?? throw new ArgumentException(nameof(logger));

        /// <summary>
        /// Registers a new motorcycle rental after validating the provided data and business rules.
        /// </summary>
        /// <param name="createDto">DTO containing rental information.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        public async Task<Result<string>> RegisterRental(MotocycleRentalCreateDto createDto)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(RegisterRental);

            if (!IsValidMotocycleRentalDto(createDto))
            {
                _logger.LogWarning(correlationId, userId, methodName, "Invalid rental DTO.");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var deliveryRider = await _deliveryPartnerRepository.GetDeliveryPartnerByIdentifier(createDto.IdDeliveryPartner);
            if (deliveryRider is null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Delivery partner not found: {createDto.IdDeliveryPartner}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            if (!deliveryRider.DriverLicenseType.Contains(nameof(LicenseDriverType.A), StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(correlationId, userId, methodName,
                    $"Delivery partner does not have required license type A. DeliveryPartnerId: {createDto.IdDeliveryPartner}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var motocycle = await _motocycleRepository.GetMotocycleByIdentifier(createDto.IdMotocycle);
            if (motocycle is null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Motocycle not found: {createDto.IdMotocycle}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            if (!(createDto.StartDate.Date == motocycle.CreatedAt.AddDays(1)))
            {
                _logger.LogWarning(correlationId, userId, methodName, 
                    $"Rental start date is not the first day after motocycle creation. StartDate: {createDto.StartDate.Date}, Expected: {motocycle.CreatedAt.AddDays(1).Date}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var (totalRentValue, dailyRate) = await _rentalPlanService.GetRentalValue(createDto.Plan, createDto.Plan);
            if (totalRentValue is null || dailyRate is null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Rental plan not found, Expected: {motocycle.CreatedAt.AddDays(1).Date}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var existingRental = await _motocycleRentalRepository.GetMotocycleRentalByMotocycleId(createDto.IdMotocycle);
            if (existingRental != null && existingRental.ExpectedEndDate >= createDto.StartDate)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Motocycle is already rented. MotocycleId: {createDto.IdMotocycle}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var motocycleRentalEntity = MotocycleRentalEntityAggregateRoot.CreateMotocycleRentalEntity(
                createDto.StartDate,
                createDto.EndDate,
                createDto.ExpectedEndDate,
                createDto.IdDeliveryPartner,
                createDto.IdMotocycle,
                createDto.Plan,
                (decimal)dailyRate,
                (decimal)totalRentValue);

            await _motocycleRentalRepository.RegisterMotocycleRental(motocycleRentalEntity);
            _logger.LogInformation(correlationId, userId, methodName, $"Rental registered successfully. DeliveryPartnerId: {createDto.IdDeliveryPartner}, MotocycleId: {createDto.IdMotocycle}");
            return Result<string>.Ok();
        }

        /// <summary>
        /// Retrieves a motorcycle rental entity by its identifier.
        /// </summary>
        /// <param name="identifier">Unique identifier of the rental.</param>
        /// <returns>Result containing the rental DTO if found, or failure if not found.</returns>
        public async Task<Result<MotocycleRentalDto>> GetMotocycleRentalByIdentifier(string identifier)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(GetMotocycleRentalByIdentifier);

            if (string.IsNullOrWhiteSpace(identifier))
            {
                _logger.LogWarning(correlationId, userId, methodName, "Identifier is null or empty.");
                return Result<MotocycleRentalDto>.Fail(Resource.INVALID_DATA);
            }

            var entity = await _motocycleRentalRepository.GetMotocycleRentalByIdentifier(identifier);
            if (entity is null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"MotocycleRentalEntity not found for Identifier: {identifier}");
                return Result<MotocycleRentalDto>.Fail(Resource.INVALID_DATA);
            }

            _logger.LogInformation(correlationId, userId, methodName, $"MotocycleRentalEntity found for Identifier: {identifier}");
            return Result<MotocycleRentalDto>.Ok(_mapper.Map<MotocycleRentalDto>(entity));
        }

        /// <summary>
        /// Updates the expected end date of a rental and calculates additional charges or penalties.
        /// </summary>
        /// <param name="identifier">Identifier of the rental.</param>
        /// <param name="newExpectedEndDate">New expected end date.</param>
        /// <returns>Result with the updated rental and any additional charges or penalties.</returns>
        public async Task<Result<MotocycleRentalDto>> UpdateExpectedEndDateAsync(string identifier, DateTime newExpectedEndDate)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(UpdateExpectedEndDateAsync);

            var rental = await _motocycleRentalRepository.GetMotocycleRentalByIdentifier(identifier);
            if (rental == null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Rental not found: {identifier}");
                return Result<MotocycleRentalDto>.Fail(Resource.INVALID_DATA);
            }

            var now = BrasiliaTimeProvider.Now;
            if (newExpectedEndDate <= now)
            {
                _logger.LogWarning(correlationId, userId, methodName, "New expected end date must be in the future.");
                return Result<MotocycleRentalDto>.Fail(Resource.INVALID_DATA);
            }

            if (rental.EndDate <= now)
            {
                _logger.LogWarning(correlationId, userId, methodName, "Rental is already closed.");
                return Result<MotocycleRentalDto>.Fail(Resource.INVALID_DATA);
            }

            decimal additionalCharge = 0;
            string details = string.Empty;

            if (newExpectedEndDate < rental.ExpectedEndDate)
            {
                var unusedDays = (rental.ExpectedEndDate.Date - newExpectedEndDate.Date).Days;
                var penaltyRate = rental.Plan == 7 ? 0.20m : rental.Plan == 15 ? 0.40m : 0m;
                var penalty = unusedDays * rental.DailyRent * penaltyRate;
                additionalCharge = unusedDays * rental.DailyRent + penalty;
                details = $"Early return. Diárias não efetivadas: {unusedDays}, Multa: {penalty:C}";
            }
            else if (newExpectedEndDate > rental.ExpectedEndDate)
            {
                var extraDays = (newExpectedEndDate.Date - rental.ExpectedEndDate.Date).Days;
                additionalCharge = extraDays * 50m;
                details = $"Late return. Diárias adicionais: {extraDays}, Valor adicional: {additionalCharge:C}";
            }
            else
            {
                details = "No additional charges.";
            }

            rental.ExpectedEndDate = newExpectedEndDate;
            await _motocycleRentalRepository.RegisterMotocycleRental(rental);

            var dto = _mapper.Map<MotocycleRentalDto>(rental);
            _logger.LogInformation(correlationId, userId, methodName, $"Expected end date updated. {details}");
            return Result<MotocycleRentalDto>.Ok(dto, details);
        }

        /// <summary>
        /// Validates the motorcycle rental creation DTO for required fields and business rules.
        /// </summary>
        /// <param name="createDto">DTO containing rental information.</param>
        /// <returns>True if the DTO is valid; otherwise, false.</returns>
        private static bool IsValidMotocycleRentalDto(MotocycleRentalCreateDto createDto)
        {
            return
                createDto != null &&
                !string.IsNullOrWhiteSpace(createDto.IdDeliveryPartner) &&
                !string.IsNullOrWhiteSpace(createDto.IdMotocycle) &&
                createDto.StartDate != default &&
                createDto.EndDate != default &&
                createDto.ExpectedEndDate != default &&
                createDto.EndDate > createDto.StartDate &&
                createDto.ExpectedEndDate >= createDto.StartDate &&
                createDto.ExpectedEndDate <= createDto.EndDate &&
                createDto.Plan != 0;
        }
    }
}
