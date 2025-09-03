namespace MotoManager.UnitTests.Shared
{
    /// <summary>
    /// Unit tests for the StringSanitizer extension.
    /// </summary>
    public class StringSanitizerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("short")] // <= 10 chars
        public void Sanitize_ShortOrNull_ReturnsInput(string input)
        {
            Assert.Equal(input, input.Sanitize());
        }

        [Fact]
        public void Sanitize_RemovesNonAlphanumeric()
        {
            var input = "abc123!@#-_=+()[]{}";
            var expected = "abc123";
            Assert.Equal(expected, input.Sanitize());
        }

        [Fact]
        public void Sanitize_TrimsResult()
        {
            var input = "   abc123!@#   ";
            var expected = "abc123";
            Assert.Equal(expected, input.Sanitize());
        }

        [Fact]
        public void Sanitize_LongString_RemovesNonAlphanumeric()
        {
            var input = "abc1234567890!@#-_=+()[]{}";
            var expected = "abc1234567890";
            Assert.Equal(expected, input.Sanitize());
        }
    }
}
