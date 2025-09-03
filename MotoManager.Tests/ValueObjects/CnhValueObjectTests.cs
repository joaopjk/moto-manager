namespace MotoManager.UnitTests.ValueObjects
{
    /// <summary>
    /// Unit tests for the CnhValueObject class.
    /// </summary>
    public class CnhValueObjectTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("11111111111")]
        [InlineData("12345678901")]
        [InlineData("abcdefghijk")]
        public void IsValid_InvalidCNH_ReturnsFalse(string cnh)
        {
            Assert.False(CnhValueObject.IsValid(cnh));
        }

        [Fact]
        public void Constructor_ValidCNH_SetsValue()
        {
            var cnh = new CnhValueObject("23588567740");
            Assert.Equal("23588567740", cnh.Value);
        }

        [Fact]
        public void Constructor_InvalidCNH_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CnhValueObject("11111111111"));
        }
    }
}
