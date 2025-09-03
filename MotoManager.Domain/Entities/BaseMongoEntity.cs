namespace MotoManager.Domain.Entities
{
    /// <summary>
    /// Base class for MongoDB entities, providing common properties.
    /// </summary>
    public abstract class BaseMongoEntity
    {
        /// <summary>
        /// MongoDB document identifier.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        /// <summary>
        /// Date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = BrasiliaTimeProvider.Now;
        /// <summary>
        /// Date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// User identifier associated with the entity.
        /// </summary>
        public string UserId { get; set; }
    }
}
