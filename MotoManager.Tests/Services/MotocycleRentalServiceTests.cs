namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the MotocycleRentalService class.
    /// </summary>
    public class MotocycleRentalServiceTests
    {
        private readonly Mock<IMotocycleRepository> _motocycleRepoMock = new();
        private readonly Mock<IDeliveryPartnerRepository> _deliveryPartnerRepoMock = new();
        private readonly Mock<IRentalPlanService> _rentalPlanServiceMock = new();
        private readonly Mock<IMotocycleRentalRepository> _motocycleRentalRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IHeaderService> _headerServiceMock = new();
        private readonly Mock<ILogger<MotocycleRentalService>> _loggerBaseMock = new();

        private MotocycleRentalService CreateService()
        {
            _headerServiceMock.Setup(h => h.GetRequestInfo()).Returns(("corr-id", "user-id"));
            var baseLogger = new BaseLogger<MotocycleRentalService>(_loggerBaseMock.Object);
            return new MotocycleRentalService(
                _motocycleRepoMock.Object,
                _rentalPlanServiceMock.Object,
                _headerServiceMock.Object,
                _deliveryPartnerRepoMock.Object,
                _motocycleRentalRepoMock.Object,
                _mapperMock.Object,
                baseLogger);
        }

        [Fact]
        public async Task RegisterRental_InvalidDto_ReturnsFail()
        {
            var service = CreateService();
            var dto = new MotocycleRentalCreateDto();
            var result = await service.RegisterRental(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterRental_DeliveryPartnerNotFound_ReturnsFail()
        {
            var service = CreateService();
            var dto = GetValidDto();
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync((DeliveryPartnerEntity)null);
            var result = await service.RegisterRental(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterRental_DeliveryPartnerWithoutLicenseA_ReturnsFail()
        {
            var service = CreateService();
            var dto = GetValidDto();
            var partner = new DeliveryPartnerEntity { DriverLicenseType = "B" };
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync(partner);
            var result = await service.RegisterRental(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterRental_MotocycleNotFound_ReturnsFail()
        {
            var service = CreateService();
            var dto = GetValidDto();
            var partner = new DeliveryPartnerEntity { DriverLicenseType = "A" };
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync(partner);
            _motocycleRepoMock.Setup(r => r.GetMotocycleByIdentifier(dto.IdMotocycle)).ReturnsAsync((MotocycleEntity)null);
            var result = await service.RegisterRental(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterRental_StartDateNotFirstDayAfterCreation_ReturnsFail()
        {
            var service = CreateService();
            var dto = GetValidDto();
            var partner = new DeliveryPartnerEntity { DriverLicenseType = "A" };
            var moto = new MotocycleEntity { CreatedAt = DateTime.Today };
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync(partner);
            _motocycleRepoMock.Setup(r => r.GetMotocycleByIdentifier(dto.IdMotocycle)).ReturnsAsync(moto);
            // StartDate diferente do esperado
            dto.StartDate = DateTime.Today.AddDays(2);
            var result = await service.RegisterRental(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterRental_RegisterSuccess_ReturnsOk()
        {
            var service = CreateService();
            var dto = GetValidDto();
            var partner = new DeliveryPartnerEntity { DriverLicenseType = "A" };
            var moto = new MotocycleEntity { CreatedAt = DateTime.Today };
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync(partner);
            _motocycleRepoMock.Setup(r => r.GetMotocycleByIdentifier(dto.IdMotocycle)).ReturnsAsync(moto);
            dto.StartDate = moto.CreatedAt.AddDays(1);
            _rentalPlanServiceMock.Setup(r => r.GetRentalValue(dto.Plan, dto.Plan)).ReturnsAsync((100m, 10m));
            _motocycleRentalRepoMock.Setup(r => r.RegisterMotocycleRental(It.IsAny<MotocycleRentalEntity>())).ReturnsAsync(true);
            var result = await service.RegisterRental(dto);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task RegisterRental_RentalPlanValuesAreNull_ReturnsFail()
        {
            var service = CreateService();
            var dto = new MotocycleRentalCreateDto
            {
                IdDeliveryPartner = "partnerId",
                IdMotocycle = "motoId",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(10),
                ExpectedEndDate = DateTime.Today.AddDays(10),
                Plan = 10
            };
            var partner = new DeliveryPartnerEntity { DriverLicenseType = "A" };
            var moto = new MotocycleEntity { CreatedAt = DateTime.Today };
            _deliveryPartnerRepoMock.Setup(r => r.GetDeliveryPartnerByIdentifier(dto.IdDeliveryPartner)).ReturnsAsync(partner);
            _motocycleRepoMock.Setup(r => r.GetMotocycleByIdentifier(dto.IdMotocycle)).ReturnsAsync(moto);
            _rentalPlanServiceMock.Setup(r => r.GetRentalValue(dto.Plan, dto.Plan)).ReturnsAsync((null, null));

            var result = await service.RegisterRental(dto);

            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task GetMotocycleRentalByIdentifier_IdentifierIsNullOrEmpty_ReturnsFail()
        {
            var service = CreateService();
            var result = await service.GetMotocycleRentalByIdentifier("");
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task GetMotocycleRentalByIdentifier_EntityNotFound_ReturnsFail()
        {
            var service = CreateService();
            _motocycleRentalRepoMock.Setup(r => r.GetMotocycleRentalByIdentifier("id123")).ReturnsAsync((MotocycleRentalEntity)null);
            var result = await service.GetMotocycleRentalByIdentifier("id123");
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task GetMotocycleRentalByIdentifier_EntityFound_ReturnsOkWithMappedDto()
        {
            var service = CreateService();
            var entity = new MotocycleRentalEntity
            {
                Identifier = "id123",
                IdDeliveryPartner = "partnerId",
                IdMotocycle = "motoId",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(10),
                ExpectedEndDate = DateTime.Today.AddDays(10),
                Plan = 10,
                DailyRent = 10m,
                TotalRentValue = 100m
            };
            var dto = new MotocycleRentalDto
            {
                Identifier = "id123",
                IdDeliveryPartner = "partnerId",
                IdMotocycle = "motoId",
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ExpectedEndDate = entity.ExpectedEndDate,
                Plan = entity.Plan,
                DailyRent = entity.DailyRent,
            };
            _motocycleRentalRepoMock.Setup(r => r.GetMotocycleRentalByIdentifier("id123")).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<MotocycleRentalDto>(entity)).Returns(dto);
            var result = await service.GetMotocycleRentalByIdentifier("id123");
            Assert.True(result.Success);
            Assert.Equal(dto.Identifier, result.Value.Identifier);
            Assert.Equal(dto.IdDeliveryPartner, result.Value.IdDeliveryPartner);
            Assert.Equal(dto.IdMotocycle, result.Value.IdMotocycle);
            Assert.Equal(dto.StartDate, result.Value.StartDate);
            Assert.Equal(dto.EndDate, result.Value.EndDate);
            Assert.Equal(dto.ExpectedEndDate, result.Value.ExpectedEndDate);
            Assert.Equal(dto.Plan, result.Value.Plan);
            Assert.Equal(dto.DailyRent, result.Value.DailyRent);
        }

        private MotocycleRentalCreateDto GetValidDto()
        {
            return new MotocycleRentalCreateDto
            {
                IdDeliveryPartner = "partnerId",
                IdMotocycle = "motoId",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(10),
                ExpectedEndDate = DateTime.Today.AddDays(10),
                Plan = 10
            };
        }
    }
}
