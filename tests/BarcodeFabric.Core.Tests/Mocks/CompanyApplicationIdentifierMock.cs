using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeFabric.Parser.Tests.Mocks
{
    internal class CompanyApplicationIdentifierMock : ApplicationIdentifier
    {
        public CompanyApplicationIdentifierMock(string identifier, string description, int min, int max) : base(identifier, description, min, max)
        {
        }

        public CompanyApplicationIdentifierMock(bool hasVariable, string identifier, string description, int min, int max) : base(hasVariable, identifier, description, min, max)
        {
        }

        #region Overrides of ApplicationIdentifier

        public override DataFormatType DataFormat { get; protected set; } = DataFormatType.Alphanumeric;
        public override object Parse()
        {
            return ElementData;
        }

        #endregion
    }
}
