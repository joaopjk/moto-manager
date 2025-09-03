namespace MotoManager.UnitTests.Shared
{
    /// <summary>
    /// Unit tests for the DataMasking utility.
    /// </summary>
    public class DataMaskingTests
    {
        [Fact]
        public void Mask_NullOrWhiteSpace_ReturnsInput()
        {
            Assert.Null(DataMasking.Mask(null));
            Assert.Equal("", DataMasking.Mask(""));
            Assert.Equal("   ", DataMasking.Mask("   "));
        }

        [Fact]
        public void Mask_Cnpj_MasksCorrectly()
        {
            var input = "CNPJ: 12345678000195";
            var masked = DataMasking.Mask(input);
            Assert.Contains("*******0195", masked);
        }

        [Fact]
        public void Mask_Cnh_MasksCorrectly()
        {
            var input = "CNH: 12345678909";
            var masked = DataMasking.Mask(input);
            Assert.Contains("*******8909", masked);
        }

        [Fact]
        public void Mask_MultipleDocuments_MasksAll()
        {
            var input = "CNPJ: 12345678000195 CNH: 12345678909";
            var masked = DataMasking.Mask(input);
            Assert.Contains("*******0195", masked);
            Assert.Contains("*******8909", masked);
        }

        [Fact]
        public void Mask_NoDocument_ReturnsOriginal()
        {
            var input = "No sensitive data here";
            var masked = DataMasking.Mask(input);
            Assert.Equal(input, masked);
        }
    }
}
