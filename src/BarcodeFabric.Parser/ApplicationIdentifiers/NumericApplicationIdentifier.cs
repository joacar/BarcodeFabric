namespace BarcodeFabric.Parser
{
    public class NumericApplicationIdentifier : ApplicationIdentifier
    {
        public NumericApplicationIdentifier(string identifier, string description, int min, int max)
            : base(identifier, description, min, max)
        {
        }

        public NumericApplicationIdentifier(bool hasVariable, string identifier, string description, int min, int max)
            : base(hasVariable, identifier, description, min, max)
        {
        }

        #region Overrides of ApplicationIdentifier

        public override DataFormatType DataFormat { get; protected set; } = DataFormatType.Numeric;

        public override object Parse()
        {
            // TODO: Validate with regex
            if (ElementData.Length < Min || ElementData.Length > Max)
            {
                throw new InvalidElementDataException($"Element data '{ElementData}' must be in range [{Min}, {Max}]");
            }
            return ElementData;
        }

        #endregion
    }
}