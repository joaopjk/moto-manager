namespace MotoManager.UnitTests.Enums.Parses
{
    /// <summary>
    /// Unit tests for the ParseCnhType utility.
    /// </summary>
    public class ParseCnhTypeTests
    {
        [Theory]
        [InlineData("A", LicenseDriverType.A)]
        [InlineData("a", LicenseDriverType.A)]
        [InlineData("B", LicenseDriverType.B)]
        [InlineData("b", LicenseDriverType.B)]
        [InlineData("A+B", LicenseDriverType.AB)]
        [InlineData("a+b", LicenseDriverType.AB)]
        [InlineData("A+B ", LicenseDriverType.AB)]
        [InlineData("  A+B", LicenseDriverType.AB)]
        public void Parse_ValidValues_ReturnsExpectedEnum(string input, LicenseDriverType expected)
        {
            var result = ParseCnhType.Parse(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("X")]
        [InlineData(null)]
        public void Parse_InvalidValues_ReturnsInvalid(string input)
        {
            var result = ParseCnhType.Parse(input ?? "");
            Assert.Equal(LicenseDriverType.Invalid, result);
        }
    }
}
