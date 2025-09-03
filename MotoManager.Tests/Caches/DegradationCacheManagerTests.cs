namespace MotoManager.UnitTests.Caches
{
    /// <summary>
    /// Unit tests for the DegradationCacheManager class.
    /// </summary>
    public class DegradationCacheManagerTests
    {
        [Fact]
        public async Task GetOrSet_ReturnsValueFromMemoryCache()
        {
            // Arrange
            var expected = "memory-value";
            var memoryCacheMock = new Mock<IMemoryCacheHelper>();
            var redisCacheMock = new Mock<IRedisCacheHelper>();
            memoryCacheMock.Setup(m => m.GetOrSet(It.IsAny<string>(), It.IsAny<Func<Task<string>>>(), It.IsAny<TimeSpan?>()))
                .ReturnsAsync(expected);
            var manager = new DegradationCacheManager(memoryCacheMock.Object, redisCacheMock.Object);

            // Act
            var result = await manager.GetOrSet("key", () => Task.FromResult("fetch-value"));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetOrSet_ReturnsValueFromRedisCache_WhenNotInMemory()
        {
            // Arrange
            var expected = "redis-value";
            var memoryCacheMock = new Mock<IMemoryCacheHelper>();
            var redisCacheMock = new Mock<IRedisCacheHelper>();
            memoryCacheMock.Setup(m => m.GetOrSet(It.IsAny<string>(), It.IsAny<Func<Task<string>>>(), It.IsAny<TimeSpan?>()))
                .Returns<string, Func<Task<string>>, TimeSpan?>((key, func, expiry) => func());
            redisCacheMock.Setup(r => r.GetOrSet(It.IsAny<string>(), It.IsAny<Func<Task<string>>>(), It.IsAny<TimeSpan?>()))
                .ReturnsAsync(expected);
            var manager = new DegradationCacheManager(memoryCacheMock.Object, redisCacheMock.Object);

            // Act
            var result = await manager.GetOrSet("key", () => Task.FromResult("fetch-value"));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetOrSet_UsesFetchFunc_WhenNotInAnyCache()
        {
            // Arrange
            var expected = "fetch-value";
            var memoryCacheMock = new Mock<IMemoryCacheHelper>();
            var redisCacheMock = new Mock<IRedisCacheHelper>();
            memoryCacheMock.Setup(m => m.GetOrSet(It.IsAny<string>(), It.IsAny<Func<Task<string>>>(), It.IsAny<TimeSpan?>()))
                .Returns<string, Func<Task<string>>, TimeSpan?>((key, func, expiry) => func());
            redisCacheMock.Setup(r => r.GetOrSet(It.IsAny<string>(), It.IsAny<Func<Task<string>>>(), It.IsAny<TimeSpan?>()))
                .Returns<string, Func<Task<string>>, TimeSpan?>((key, func, expiry) => func());
            var manager = new DegradationCacheManager(memoryCacheMock.Object, redisCacheMock.Object);

            // Act
            var result = await manager.GetOrSet("key", () => Task.FromResult(expected));

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
