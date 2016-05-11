using Xunit;

namespace BarcodeFabric.Parser.Tests
{
    public class GtinParserFixture
    {
        [Theory]
        [InlineData("2388060112344", 1.234d, "2388060100004")]
        [InlineData("238806011234", 1.234d, "238806010000")]
        public void Parse_Gtin_WeightPrefix(string code, double data, string gtin)
        {
            var parser = new GtinParser(code);
            var barcode = parser.Parse();
            Assert.Equal(barcode["wpd"], data);
            Assert.Equal(barcode["gtin"], gtin);
        }
    }
}