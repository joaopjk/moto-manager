namespace MotoManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for motorcycle entities using MongoDB.
    /// </summary>
    public class MotocycleRepository(IMongoContext context) : IMotocycleRepository
    {
        private const string CollectionName = "Motocycles";
        private readonly IMongoCollection<MotocycleEntity> _collection = context.GetCollection<MotocycleEntity>(CollectionName) ?? throw new ArgumentException(nameof(context));

        /// <summary>
        /// Retrieves a motorcycle entity by its plate.
        /// </summary>
        public async Task<MotocycleEntity> GetMotocycleByPlate(string plate)
        {
            var filter = Builders<MotocycleEntity>.Filter.Eq(x => x.Plate, plate);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Registers a new motorcycle entity.
        /// </summary>
        public async Task<bool> RegisterMotocycle(MotocycleEntity motocycle)
        {
            await _collection.InsertOneAsync(motocycle);
            return true;
        }

        /// <summary>
        /// Retrieves a motorcycle entity by its identifier.
        /// </summary>
        public async Task<MotocycleEntity> GetMotocycleByIdentifier(string identifier)
        {
            var filter = Builders<MotocycleEntity>.Filter.Eq(x => x.Identifier, identifier);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all motorcycle entities.
        /// </summary>
        public async Task<IEnumerable<MotocycleEntity>> GetAllMotocycle()
        {
            return await _collection.Find(Builders<MotocycleEntity>.Filter.Empty).ToListAsync();
        }

        /// <summary>
        /// Deletes a motorcycle entity by its identifier.
        /// </summary>
        public async Task<bool> DeleteMotocycle(string identifier)
        {
            var filter = Builders<MotocycleEntity>.Filter.Eq(x => x.Identifier, identifier);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Updates only the plate of an existing motorcycle entity by its identifier.
        /// </summary>
        /// <param name="motocycle">Motorcycle entity with updated plate and identifier.</param>
        /// <returns>True if update is successful; otherwise, false.</returns>
        public async Task<bool> UpdateMotocyclePlate(MotocycleEntity motocycle)
        {
            var filter = Builders<MotocycleEntity>.Filter.Eq(x => x.Identifier, motocycle.Identifier);
            var update = Builders<MotocycleEntity>.Update
                .Set(x => x.Plate, motocycle.Plate)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
