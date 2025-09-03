namespace MotoManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for rental plan entities using MongoDB.
    /// </summary>
    public class RentalPlanRepository(IMongoContext context) : IRentalPlanRepository
    {
        private const string CollectionName = "RetalPlans";
        private readonly IMongoCollection<RentalPlanEntity> _collection =
            context.GetCollection<RentalPlanEntity>(CollectionName) ?? throw new ArgumentException(nameof(context));

        /// <summary>
        /// Registers a new rental plan entity.
        /// </summary>
        public async Task<bool> RegisterRentalPlan(RentalPlanEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Ensures that the default rental plans are present in the repository.
        /// </summary>
        public async Task EnsureDefaultRentalPlans(IEnumerable<RentalPlanEntity> defaultPlans)
        {
            var count = await _collection.CountDocumentsAsync(Builders<RentalPlanEntity>.Filter.Empty);
            if (count == 0)
                await _collection.InsertManyAsync(defaultPlans);
        }

        /// <summary>
        /// Retrieves all rental plan entities.
        /// </summary>
        public async Task<IEnumerable<RentalPlanEntity>> GetAllRentalPlans()
        {
            var plans = await _collection.Find(Builders<RentalPlanEntity>.Filter.Empty).ToListAsync();
            return plans;
        }
    }
}
