namespace MotoManager.Infrastructure.Context
{
    /// <summary>
    /// MongoDB context implementation for accessing collections.
    /// </summary>
    public class MongoContext(IMongoClient client) : IMongoContext
    {
        private readonly IMongoDatabase _database = client.GetDatabase(MongoConstants.DatabaseName) ?? throw new ArgumentNullException(nameof(client));

        /// <summary>
        /// Gets a MongoDB collection by name for the specified type.
        /// </summary>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
