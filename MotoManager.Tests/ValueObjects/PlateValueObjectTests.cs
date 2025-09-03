namespace MotoManager.UnitTests.ValueObjects
{
    /// <summary>
    /// Unit tests for the PlateValueObject class.
    /// </summary>
    public class PlateValueObjectTests
    {
        [Theory]
        [InlineData("ABC1234")]
        [InlineData("ABC-1234")]
        [InlineData("ABC1A23")]
        public void IsValid_ValidPlate_ReturnsTrue(string plate)
        {
            Assert.True(PlateValueObject.IsValid(plate));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("A1B2C3D4")]
        [InlineData("12345678")]
        public void IsValid_InvalidPlate_ReturnsFalse(string plate)
        {
            Assert.False(PlateValueObject.IsValid(plate));
        }

        [Fact]
        public void Constructor_ValidPlate_SetsValue()
        {
            var plate = new PlateValueObject("ABC1234");
            Assert.Equal("ABC1234", plate.Value);
        }

        [Fact]
        public void Constructor_InvalidPlate_ValueIsNull()
        {
            var plate = new PlateValueObject("");
            Assert.Null(plate.Value);
        }
    }
}
