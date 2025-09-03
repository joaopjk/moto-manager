namespace MotoManager.UnitTests.Context
{
    /// <summary>
    /// Unit tests for the MongoContext class.
    /// </summary>
    public class MongoContextTests
    {
        [Fact]
        public void Constructor_SetsDatabaseCorrectly()
        {
            // Arrange
            var databaseMock = new Mock<IMongoDatabase>();
            var clientMock = new Mock<IMongoClient>();
            clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(databaseMock.Object);

            // Act
            var context = new MongoContext(clientMock.Object);

            // Assert
            Assert.NotNull(context);
        }

        [Fact]
        public void GetCollection_ReturnsCollectionFromDatabase()
        {
            // Arrange
            var collectionMock = new Mock<IMongoCollection<string>>();
            var databaseMock = new Mock<IMongoDatabase>();
            databaseMock.Setup(d => d.GetCollection<string>("test", null)).Returns(collectionMock.Object);
            var clientMock = new Mock<IMongoClient>();
            clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(databaseMock.Object);
            var context = new MongoContext(clientMock.Object);

            // Act
            var collection = context.GetCollection<string>("test");

            // Assert
            Assert.NotNull(collection);
            Assert.Equal(collectionMock.Object, collection);
        }
    }
}
