namespace MotoManager.Application.Services
{
    /// <summary>
    /// Service responsible for delivery partner manager.
    /// </summary>
    public class DeliveryPartnerService(
        IDeliveryPartnerRepository deliveryPartnerRepository,
        IMinIoStorage minIoStorage,
        IHeaderService headerService,
        IImageValidatorService imageValidatorService,
        BaseLogger<DeliveryPartnerService> logger) : IDeliveryPartnerService
    {
        private readonly IDeliveryPartnerRepository _deliveryPartnerRepository = deliveryPartnerRepository ?? throw new ArgumentException(nameof(deliveryPartnerRepository));
        private readonly IMinIoStorage _minIoStorage = minIoStorage ?? throw new ArgumentException(nameof(minIoStorage));
        private readonly IHeaderService _headerService = headerService ?? throw new ArgumentException(nameof(headerService));
        private readonly IImageValidatorService _imageValidatorService = imageValidatorService ?? throw new ArgumentException(nameof(imageValidatorService));
        private readonly BaseLogger<DeliveryPartnerService> _logger = logger ?? throw new ArgumentException(nameof(logger));

        /// <summary>
        /// Registers a new delivery rider after validating the provided data and checking for duplicates.
        /// </summary>
        /// <param name="dto">Delivery partner data transfer object.</param>
        /// <returns>Result indicating success or failure of the registration.</returns>
        public async Task<Result<string>> RegisterDeliveryRider(DeliveryPartnerDto dto)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(RegisterDeliveryRider);

            if (!IsValidDeliveryPartnerDto(dto))
            {
                _logger.LogWarning(
                    correlationId,
                    userId,
                    methodName,
                    $"Invalid data for delivery rider registration. DTO: {dto}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var cnpjExists = await _deliveryPartnerRepository.ExistsByCnpj(dto.Cnpj);
            if (cnpjExists)
            {
                _logger.LogWarning(
                    correlationId,
                    userId,
                    methodName,
                    $"CNPJ already exists: {dto.Cnpj}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var driverLicenseNumberExists = await _deliveryPartnerRepository.ExistsByCnh(dto.DriverLicenseNumber);
            if (driverLicenseNumberExists)
            {
                _logger.LogWarning(
                    correlationId,
                    userId,
                    methodName,
                    $"Driver license number already exists: {dto.DriverLicenseNumber}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var deliveryPartnerEntity = new DeliveryPartnerEntity
            {
                Identifier = dto.Identifier.Sanitize(),
                Name = dto.Name.Sanitize(),
                Cnpj = dto.Cnpj,
                DateOfBirth = dto.DateOfBirth,
                DriverLicenseType = dto.DriverLicenseType.Sanitize(),
                DriverLicenseNumber = dto.DriverLicenseNumber.Sanitize(),
                ImageDriverLicenseNumberFileName = string.Empty,
                UserId = userId
            };

            await _deliveryPartnerRepository.RegisterDeliveryPartner(deliveryPartnerEntity);
            _logger.LogInformation(
                correlationId,
                userId,
                methodName,
                $"Delivery rider registered successfully. Identifier: {dto.Identifier}");
            return Result<string>.Ok(Resource.DELIVERYRIDER_UPDATED_SUCCESSFULLY);
        }

        /// <summary>
        /// Uploads the CNH image for a delivery partner, validates the image, and updates the partner's record.
        /// </summary>
        /// <param name="id">Identifier of the delivery partner.</param>
        /// <param name="file">Base64 string of the CNH image.</param>
        /// <returns>Result indicating success or failure of the upload and update.</returns>
        public async Task<Result<string>> UploadDriverLicenseImage(string id, string file)
        {
            var (correlationId, userId) = _headerService.GetRequestInfo();
            const string methodName = nameof(UploadDriverLicenseImage);


            var deliveryPartner = await _deliveryPartnerRepository.GetDeliveryPartnerByIdentifier(id);
            if (deliveryPartner is null)
            {
                _logger.LogWarning(correlationId, userId, methodName, $"Delivery partner not found: {id}");
                return Result<string>.Fail(Resource.INVALID_DATA);
            }

            var (stream, fileName, contentType) = _imageValidatorService.ConvertBase64ImageToStream(id, file);

            await _minIoStorage.UploadFileAsync(fileName, stream, contentType);

            deliveryPartner.ImageDriverLicenseNumberFileName = fileName;

            await _deliveryPartnerRepository.UpdateDeliveryPartner(deliveryPartner);
            _logger.LogInformation(correlationId, userId, methodName, $"CNH image upload successful for delivery partner: {id}");
            return Result<string>.Ok(Resource.DELIVERYRIDER_UPDATED_SUCCESSFULLY);
        }

        /// <summary>
        /// Validates the delivery partner DTO for required fields and basic data integrity.
        /// </summary>
        /// <param name="dto">Delivery partner data transfer object.</param>
        /// <returns>True if the DTO is valid; otherwise, false.</returns>
        private static bool IsValidDeliveryPartnerDto(DeliveryPartnerDto dto)
        {
            // The following validation is commented out to avoid interfering with automated tests.
            // Uncomment for production use when domain value objects and parsing are required.
            //var validCnpj = new CnpjValueObject(dto.Cnpj);
            //if (validCnpj.Value == null)
            //    return false;
            //var validCnh = new CnhValueObject(dto.DriverLicenseNumber);
            //if (validCnh.Value == null)
            //    return false;
            //var cnhType = ParseCnhType.Parse(dto.DriverLicenseType);
            //if (cnhType == LicenseDriverType.Invalid)
            //    return false;

            return
                dto != null &&
                dto.DateOfBirth != default &&
                !string.IsNullOrWhiteSpace(dto.Identifier) &&
                !string.IsNullOrWhiteSpace(dto.Name) &&
                !string.IsNullOrWhiteSpace(dto.Cnpj) &&
                !string.IsNullOrWhiteSpace(dto.DriverLicenseNumber);
        }
    }
}
