using System;
using BarcodeFabric.Parser.Tests.Mocks;
using Xunit;

namespace BarcodeFabric.Parser.Tests
{
    public class Gs1128ParserFixture
    {
        [Fact]
        public void Constructor_Argument_Invalid_Empty()
        {
            Assert.Throws<ArgumentException>(() => new Gs1128Parser(" "));
        }

        [Fact]
        public void Constructor_Argument_Invalid_Null()
        {
            Assert.Throws<ArgumentException>(() => new Gs1128Parser(null));
        }

        [Fact]
        public void Parse_Gs1_128_ApplicationIdentifierVariable()
        {
            const double weight = 0.480d;
            var data = $"{ApplicationIdentifierManager.Gtin}00238806011234{ApplicationIdentifierManager.ProductionNetWeight}3000480";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            var ai = (ApplicationIdentifier)barcode[ApplicationIdentifierManager.ProductionNetWeight];
            Assert.Equal(weight, ai.Parse());
        }

        [Fact]
        public void Parse_Gs1_128_Multiple_Compact()
        {
            const string data = "11160505172005052112345";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.Equal(3, barcode.Count);
        }

        [Fact]
        public void Parse_Gs1_128_Multiple_Fnc1()
        {
            var data = $"2112345{Gs1128Parser.Fnc1}1116050517200505";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.Equal(3, barcode.Count);
        }

        [Fact]
        public void Parse_Gs1_128_Multiple_Sscc18_ProductionDate()
        {
            const string productionDate = "160505";
            const string sscc = "123456789101112131";
            var data = $"{ApplicationIdentifierManager.Sscc}{sscc}{ApplicationIdentifierManager.ProductionDate}{productionDate}";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.NotNull(barcode);
            Assert.Equal(2, barcode.Count);
            var ai = (ApplicationIdentifier)barcode[ApplicationIdentifierManager.Sscc];
            Assert.Equal(ApplicationIdentifierManager.Sscc, ai.Identifier);
            Assert.Equal(sscc, ai.Parse());

            ai = (ApplicationIdentifier)barcode[ApplicationIdentifierManager.ProductionDate];
            Assert.Equal(ApplicationIdentifierManager.ProductionDate, ai.Identifier);
            Assert.Equal(new DateTime(2016, 5, 5).Date, ai.Parse());
        }

        [Fact]
        public void Parse_Gs1_128_Sssc14()
        {
            const string sscc = "12345678910111";
            var data = $"{ApplicationIdentifierManager.Gtin}{sscc}";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.NotNull(barcode);
            Assert.Equal(1, barcode.Count);
            var ai = (ApplicationIdentifier)barcode[ApplicationIdentifierManager.Gtin];
            Assert.Equal(ApplicationIdentifierManager.Gtin, ai.Identifier);
            Assert.Equal(sscc, ai.Parse());
        }

        [Fact]
        public void Parse_Gs1_128_Sssc18()
        {
            const string sscc = "123456789101112131";
            var data = $"{ApplicationIdentifierManager.Sscc}{sscc}";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.NotNull(barcode);
            Assert.Equal(1, barcode.Count);
            var ai = (ApplicationIdentifier)barcode[ApplicationIdentifierManager.Sscc];
            Assert.Equal(ApplicationIdentifierManager.Sscc, ai.Identifier);
            Assert.Equal(sscc, ai.Parse());
        }

        [Fact]
        public void AddCompanyInternalApplicationIdentifier()
        {
            const ushort company = 91;
            ApplicationIdentifierManager.AddApplicationIdentifier(company, () => new CompanyApplicationIdentifierMock("91", "Company internal 91", 1, 30));
            const string data = "91MyCustomData";
            var parser = new Gs1128Parser(data);
            var barcode = parser.Parse();
            Assert.Equal(1, barcode.Count);
            Assert.IsType<CompanyApplicationIdentifierMock>(barcode["91"]);
            var ai = (CompanyApplicationIdentifierMock)barcode["91"];
            Assert.Equal("91", ai.Identifier);
            Assert.Equal("MyCustomData", ai.Parse());
        }

        [Fact(Skip = "Trying to resolve how to determine if we reached end of input correctly or via " +
                     "an incorrectly determined variable length AI (missing FNC1)")]
        public void VariableLengthMissingFnc1_InvalidTokenException()
        {
            const string data = "21123451116050517200505";
            var parser = new Gs1128Parser(data);
            Assert.Throws<InvalidTokenException>(() => parser.Parse());
        }
    }
}