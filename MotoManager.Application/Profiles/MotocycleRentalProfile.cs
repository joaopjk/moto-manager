namespace MotoManager.Application.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between MotocycleRental DTOs and entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MotocycleRentalProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MotocycleRentalProfile"/> class and configures the mappings.
        /// </summary>
        public MotocycleRentalProfile()
        {
            CreateMap<MotocycleRentalCreateDto, MotocycleRentalEntity>().ReverseMap();
            CreateMap<MotocycleRentalDto, MotocycleRentalEntity>().ReverseMap();
        }
    }
}
