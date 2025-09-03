namespace MotoManager.Application.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between Motocycle DTOs, entities, and events.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MotocycleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MotocycleProfile"/> class and configures the mappings.
        /// </summary>
        public MotocycleProfile()
        {
            CreateMap<MotocycleDto, MotocycleEntity>().ReverseMap();
            CreateMap<MotocycleCreatedEvent, MotocycleEntity>().ReverseMap();
        }
    }
}
