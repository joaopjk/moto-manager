namespace MotoManager.Application.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between DeliveryPartner DTOs and entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DeliveryRiderProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRiderProfile"/> class and configures the mappings.
        /// </summary>
        public DeliveryRiderProfile()
        {
            CreateMap<DeliveryPartnerDto, DeliveryPartnerEntity>().ReverseMap();
        }
    }
}
