namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the MotocycleService class.
    /// </summary>
    public class MotocycleServiceTests
    {
        private readonly Mock<IMotocycleRepository> _repoMock = new();
        private readonly Mock<IMotocycleEventHandler> _eventHandlerMock = new();
        private readonly Mock<IMotocycleRentalRepository> _rentalRepoMock = new();
        private readonly Mock<IHeaderService> _headerServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<MotocycleService>> _loggerBaseMock = new();

        private MotocycleService CreateService()
        {
            _headerServiceMock.Setup(h => h.GetRequestInfo()).Returns(("corr-id", "user-id"));
            var baseLogger = new BaseLogger<MotocycleService>(_loggerBaseMock.Object);
            return new MotocycleService(
                _repoMock.Object,
                _eventHandlerMock.Object,
                _rentalRepoMock.Object,
                _headerServiceMock.Object,
                _mapperMock.Object,
                baseLogger);
        }

        [Fact]
        public async Task RegisterMotocycle_InvalidDto_ReturnsFail()
        {
            var service = CreateService();
            var dto = new MotocycleDto { Plate = "", Identifier = "", Model = "", Year = 2020 };
            var result = await service.RegisterMotocycle(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterMotocycle_ExistingPlate_ReturnsFail()
        {
            var service = CreateService();
            var dto = new MotocycleDto { Plate = "ABC1234", Identifier = "ID1", Model = "ModelX", Year = 2020 };
            _repoMock.Setup(r => r.GetMotocycleByPlate(dto.Plate)).ReturnsAsync(new MotocycleEntity());
            var result = await service.RegisterMotocycle(dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task RegisterMotocycle_ValidDto_ReturnsSuccess()
        {
            var service = CreateService();
            var dto = new MotocycleDto { Plate = "ABC1234", Identifier = "ID1", Model = "ModelX", Year = 2020 };
            _repoMock.Setup(r => r.GetMotocycleByPlate(dto.Plate)).ReturnsAsync((MotocycleEntity)null!);
            _eventHandlerMock.Setup(e => e.PublishMotocycleEvent(It.IsAny<MotocycleCreatedEvent>())).ReturnsAsync(true);
            var result = await service.RegisterMotocycle(dto);
            Assert.True(result.Success);
            Assert.Equal(Resource.MOTOCYCLE_REGISTERED_SUCCESSFULLY, result.Message.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetMotocycleByIdentifier_InvalidIdentifier_ReturnsFail(string identifier)
        {
            var service = CreateService();
            var result = await service.GetMotocycleByIdentifier(identifier);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_REQUEST_MESSAGE, result.Message.Message);
        }

        [Fact]
        public async Task GetMotocycleByIdentifier_NotFound_ReturnsFail()
        {
            var service = CreateService();
            var identifier = "NOTFOUND";
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync((MotocycleEntity)null!);
            var result = await service.GetMotocycleByIdentifier(identifier);
            Assert.False(result.Success);
            Assert.Equal(Resource.MOTOCYCLE_NOT_FOUND, result.Message.Message);
        }

        [Fact]
        public async Task GetMotocycleByIdentifier_Found_ReturnsSuccess()
        {
            var service = CreateService();
            var identifier = "ID123";
            var entity = new MotocycleEntity { Plate = "ABC1234", Identifier = identifier, Model = "ModelX", Year = 2020 };
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<MotocycleDto>(entity)).Returns(new MotocycleDto { Plate = entity.Plate, Identifier = entity.Identifier, Model = entity.Model, Year = entity.Year });
            var result = await service.GetMotocycleByIdentifier(identifier);
            Assert.True(result.Success);
            Assert.Equal(identifier, result.Value.Identifier);
        }

        [Fact]
        public async Task SearchMotocycle_EmptyPlate_ReturnsAll()
        {
            var service = CreateService();
            var entities = new List<MotocycleEntity> {
                new MotocycleEntity { Plate = "ABC1234", Identifier = "ID1", Model = "ModelX", Year = 2020 },
                new MotocycleEntity { Plate = "XYZ5678", Identifier = "ID2", Model = "ModelY", Year = 2021 }
            };
            _repoMock.Setup(r => r.GetAllMotocycle()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<MotocycleDto>>(entities)).Returns(entities.Select(e => new MotocycleDto { Plate = e.Plate, Identifier = e.Identifier, Model = e.Model, Year = e.Year }));
            var result = await service.SearchMotocycle("");
            Assert.True(result.Success);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task SearchMotocycle_ExistingPlate_ReturnsOne()
        {
            var service = CreateService();
            var entity = new MotocycleEntity { Plate = "ABC1234", Identifier = "ID1", Model = "ModelX", Year = 2020 };
            _repoMock.Setup(r => r.GetMotocycleByPlate("ABC1234")).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IEnumerable<MotocycleDto>>(It.IsAny<IEnumerable<MotocycleEntity>>()))
                .Returns((IEnumerable<MotocycleEntity> e) => e.Select(x => new MotocycleDto { Plate = x.Plate, Identifier = x.Identifier, Model = x.Model, Year = x.Year }));
            var result = await service.SearchMotocycle("ABC1234");
            Assert.True(result.Success);
            Assert.Single(result.Value);
            Assert.Equal("ABC1234", result.Value.First().Plate);
        }

        [Fact]
        public async Task SearchMotocycle_NonExistingPlate_ReturnsEmpty()
        {
            var service = CreateService();
            _repoMock.Setup(r => r.GetMotocycleByPlate("NOTFOUND")).ReturnsAsync((MotocycleEntity)null!);
            _mapperMock.Setup(m => m.Map<IEnumerable<MotocycleDto>>(It.IsAny<IEnumerable<MotocycleEntity>>()))
                .Returns((IEnumerable<MotocycleEntity> e) => e.Select(x => new MotocycleDto { Plate = x.Plate, Identifier = x.Identifier, Model = x.Model, Year = x.Year }));
            var result = await service.SearchMotocycle("NOTFOUND");
            Assert.True(result.Success);
            Assert.Empty(result.Value);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("ID1", null)]
        [InlineData("", "ABC1234")]
        [InlineData("ID1", "")]
        public async Task UpdatePlateMotocycle_InvalidData_ReturnsFail(string identifier, string plate)
        {
            var service = CreateService();
            var dto = plate == null ? null : new PlateDto { Plate = plate };
            var result = await service.UpdatePlateMotocycle(identifier, dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task UpdatePlateMotocycle_MotocycleNotFound_ReturnsFail()
        {
            var service = CreateService();
            var identifier = "ID_NOT_FOUND";
            var dto = new PlateDto { Plate = "ABC1234" };
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync((MotocycleEntity)null!);
            var result = await service.UpdatePlateMotocycle(identifier, dto);
            Assert.False(result.Success);
            Assert.Equal(Resource.MOTOCYCLE_NOT_FOUND, result.Message.Message);
        }

        [Fact]
        public async Task UpdatePlateMotocycle_UpdateSuccess_ReturnsSuccess()
        {
            var service = CreateService();
            var identifier = "ID1";
            var dto = new PlateDto { Plate = "ABC1234" };
            var entity = new MotocycleEntity { Identifier = identifier, Plate = "OLDPLATE", Model = "ModelX", Year = 2020 };
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateMotocyclePlate(It.IsAny<MotocycleEntity>())).ReturnsAsync(true);
            var result = await service.UpdatePlateMotocycle(identifier, dto);
            Assert.True(result.Success);
            Assert.Equal(Resource.PLATE_UPDATED_SUCCESSFULLY, result.Message.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DeleteMotocycle_InvalidIdentifier_ReturnsFail(string identifier)
        {
            var service = CreateService();
            var result = await service.DeleteMotocycle(identifier);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task DeleteMotocycle_MotocycleNotFound_ReturnsFail()
        {
            var service = CreateService();
            var identifier = "ID_NOT_FOUND";
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync((MotocycleEntity)null!);
            var result = await service.DeleteMotocycle(identifier);
            Assert.False(result.Success);
            Assert.Equal(Resource.INVALID_DATA, result.Message.Message);
        }

        [Fact]
        public async Task DeleteMotocycle_DeleteSuccess_ReturnsSuccess()
        {
            var service = CreateService();
            var identifier = "ID1";
            var entity = new MotocycleEntity { Identifier = identifier, Plate = "ABC1234", Model = "ModelX", Year = 2020 };
            var rental = new MotocycleRentalEntity { IdMotocycle = identifier };
            _repoMock.Setup(r => r.GetMotocycleByIdentifier(identifier)).ReturnsAsync(entity);
            _rentalRepoMock.Setup(r => r.GetMotocycleRentalByMotocycleId(entity.Identifier)).ReturnsAsync(rental);
            _repoMock.Setup(r => r.DeleteMotocycle(identifier)).ReturnsAsync(true);
            var result = await service.DeleteMotocycle(identifier);
            Assert.True(result.Success);
            Assert.Equal(Resource.MOTORCYCLE_REMOVED_SUCCESSFULLY, result.Message.Message);
        }
    }
}
