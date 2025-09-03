namespace MotoManager.Domain.Interfaces.Context
{
    /// <summary>
    /// Interface for MongoDB context operations, providing access to collections.
    /// </summary>
    public interface IMongoContext
    {
        /// <summary>
        /// Gets a MongoDB collection by name for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the collection documents.</typeparam>
        /// <param name="name">Name of the collection.</param>
        /// <returns>The MongoDB collection for the specified type.</returns>
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
