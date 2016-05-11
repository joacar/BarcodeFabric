using System;

namespace BarcodeFabric.Parser
{
    public class DigitApplicationIdentifier : ApplicationIdentifier
    {
        public DigitApplicationIdentifier(string identifier, string description, int min, int max)
            : base(identifier, description, min, max)
        {
        }

        public DigitApplicationIdentifier(bool hasVariable, string identifier, string description, int min, int max)
            : base(hasVariable, identifier, description, min, max)
        {
        }

        #region Overrides of ApplicationIdentifier

        public override DataFormatType DataFormat { get; protected set; } = DataFormatType.Digit;

        public override object Parse()
        {
            // TODO: Validate length
            if (HasVariable)
            {
                return int.Parse(ElementData) / Math.Pow(10, Variable);
            }
            return int.Parse(ElementData);
        }

        #endregion
    }
}