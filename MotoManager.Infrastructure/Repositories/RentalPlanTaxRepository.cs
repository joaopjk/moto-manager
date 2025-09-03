using System.Linq;

namespace MotoManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for rental plan tax entities using MongoDB.
    /// </summary>
    public class RentalPlanTaxRepository(IMongoContext context) : IRentalPlanTaxRepository
    {
        private const string CollectionName = "RentalPlanTaxes";
        private readonly IMongoCollection<RentalPlanTaxEntity> _collection = context.GetCollection<RentalPlanTaxEntity>(CollectionName) ?? throw new ArgumentException(nameof(context));

        /// <summary>
        /// Retrieves all rental plan tax entities.
        /// </summary>
        public async Task<List<RentalPlanTaxEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<RentalPlanTaxEntity>.Filter.Empty).ToListAsync();
        }

        /// <summary>
        /// Ensures that the default rental plan tax entities are present in the repository.
        /// </summary>
        /// <param name="defaultPlans">Collection of default rental plan tax entities.</param>
        public async Task EnsureDefaultRentalTaxPlansAsync(IEnumerable<RentalPlanTaxEntity> defaultPlans)
        {
            var existingPlans = await _collection.Find(Builders<RentalPlanTaxEntity>.Filter.Empty).ToListAsync();
            var existingDays = new HashSet<int>(existingPlans.Select(x => x.Days));
            var plansToInsert = defaultPlans.Where(x => !existingDays.Contains(x.Days)).ToList();
            if (plansToInsert.Any())
            {
                await _collection.InsertManyAsync(plansToInsert);
            }
        }
    }
}
