namespace MotoManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for delivery partner entities using MongoDB.
    /// </summary>
    public class DeliveryPartnerRepository(IMongoContext context) : IDeliveryPartnerRepository
    {
        private const string CollectionName = "DeliveryPartners";
        private readonly IMongoCollection<DeliveryPartnerEntity> _collection =
            context.GetCollection<DeliveryPartnerEntity>(CollectionName) ?? throw new ArgumentException(nameof(context));

        /// <summary>
        /// Checks if a delivery partner exists by CNPJ.
        /// </summary>
        public async Task<bool> ExistsByCnpj(string cnpj)
        {
            var filter = Builders<DeliveryPartnerEntity>.Filter.Eq(x => x.Cnpj, cnpj);
            return await _collection.Find(filter).AnyAsync();
        }

        /// <summary>
        /// Checks if a delivery partner exists by CNH.
        /// </summary>
        public async Task<bool> ExistsByCnh(string cnh)
        {
            var filter = Builders<DeliveryPartnerEntity>.Filter.Eq(x => x.DriverLicenseNumber, cnh);
            return await _collection.Find(filter).AnyAsync();
        }

        /// <summary>
        /// Registers a new delivery partner entity.
        /// </summary>
        public async Task<bool> RegisterDeliveryPartner(DeliveryPartnerEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Retrieves a delivery partner entity by its identifier.
        /// </summary>
        public async Task<DeliveryPartnerEntity> GetDeliveryPartnerByIdentifier(string identifier)
        {
            var filter = Builders<DeliveryPartnerEntity>.Filter.Eq(x => x.Identifier, identifier);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates a delivery partner entity.
        /// </summary>
        public async Task<bool> UpdateDeliveryPartner(DeliveryPartnerEntity deliveryPartner)
        {
            var filter = Builders<DeliveryPartnerEntity>.Filter.Eq(x => x.Identifier, deliveryPartner.Identifier);
            var updateResult = await _collection.ReplaceOneAsync(filter, deliveryPartner);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
