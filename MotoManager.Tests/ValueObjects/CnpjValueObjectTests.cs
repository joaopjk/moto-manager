namespace MotoManager.UnitTests.ValueObjects
{
    /// <summary>
    /// Unit tests for the CnpjValueObject class.
    /// </summary>
    public class CnpjValueObjectTests
    {
        [Theory]
        [InlineData("12345678000195")]
        [InlineData("00000000000191")]
        public void IsValid_ValidCnpj_ReturnsTrue(string cnpj)
        {
            Assert.True(CnpjValueObject.IsValid(cnpj));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("11111111111111")]
        [InlineData("12345678901234")]
        [InlineData("abcdefghijklm")]
        public void IsValid_InvalidCnpj_ReturnsFalse(string cnpj)
        {
            Assert.False(CnpjValueObject.IsValid(cnpj));
        }

        [Fact]
        public void Constructor_ValidCnpj_SetsValue()
        {
            var cnpj = new CnpjValueObject("12345678000195");
            Assert.Equal("12345678000195", cnpj.Value);
        }

        [Fact]
        public void Constructor_InvalidCnpj_ValueIsNull()
        {
            var cnpj = new CnpjValueObject("");
            Assert.Null(cnpj.Value);
        }
    }
}
