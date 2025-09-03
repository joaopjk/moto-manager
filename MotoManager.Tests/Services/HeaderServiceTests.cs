using Microsoft.AspNetCore.Http;

namespace MotoManager.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the HeaderService class.
    /// </summary>
    public class HeaderServiceTests
    {
        [Fact]
        public void GetCorrelationId_HeaderPresent_ReturnsValue()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["X-Correlation-Id"] = "abc-123";
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(context);

            var service = new HeaderService(accessorMock.Object);

            var result = service.GetCorrelationId();
            Assert.Equal("abc-123", result);
        }

        [Fact]
        public void GetCorrelationId_HeaderAbsent_ReturnsNull()
        {
            var context = new DefaultHttpContext();
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(context);

            var service = new HeaderService(accessorMock.Object);

            var result = service.GetCorrelationId();
            Assert.Null(result);
        }

        [Fact]
        public void GetCorrelationId_NoContext_ReturnsNull()
        {
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);

            var service = new HeaderService(accessorMock.Object);

            var result = service.GetCorrelationId();
            Assert.Null(result);
        }

        [Fact]
        public void GetUserId_ReturnsFixedValue()
        {
            var accessorMock = new Mock<IHttpContextAccessor>();
            var service = new HeaderService(accessorMock.Object);

            var result = service.GetUserId();
            Assert.Equal("9999", result);
        }

        [Fact]
        public void GetRequestInfo_ReturnsCorrelationIdAndUserId()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["X-Correlation-Id"] = "abc-123";
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(context);

            var service = new HeaderService(accessorMock.Object);

            var (correlationId, userId) = service.GetRequestInfo();
            Assert.Equal("abc-123", correlationId);
            Assert.Equal("9999", userId);
        }
    }
}
