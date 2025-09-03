namespace MotoManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for motorcycle rental entities using MongoDB.
    /// </summary>
    public class MotocycleRentalRepository(IMongoContext context) : IMotocycleRentalRepository
    {
        private const string CollectionName = "MotocycleRentals";
        private readonly IMongoCollection<MotocycleRentalEntity> _collection =
            context.GetCollection<MotocycleRentalEntity>(CollectionName) ?? throw new ArgumentException(nameof(context));

        /// <summary>
        /// Registers a new motorcycle rental entity.
        /// </summary>
        public async Task<bool> RegisterMotocycleRental(MotocycleRentalEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Retrieves a motorcycle rental entity by its identifier.
        /// </summary>
        public async Task<MotocycleRentalEntity> GetMotocycleRentalByIdentifier(string identifier)
        {
            var filter = Builders<MotocycleRentalEntity>.Filter.Eq(x => x.Identifier, identifier);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a motorcycle rental entity by the motorcycle identifier.
        /// </summary>
        /// <param name="idMotocycle">Identifier of the motorcycle.</param>
        /// <returns>The motorcycle rental entity if found; otherwise, null.</returns>
        public async Task<MotocycleRentalEntity> GetMotocycleRentalByMotocycleId(string idMotocycle)
        {
            var filter = Builders<MotocycleRentalEntity>.Filter.Eq(x => x.IdMotocycle, idMotocycle);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
