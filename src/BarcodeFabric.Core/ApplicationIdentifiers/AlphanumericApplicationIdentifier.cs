namespace BarcodeFabric.Core
{
    public class AlphanumericApplicationIdentifier : ApplicationIdentifier
    {
        public AlphanumericApplicationIdentifier(string identifier, string description, int min, int max) : base(identifier, description, min, max)
        {
        }

        public AlphanumericApplicationIdentifier(bool hasVariable, string identifier, string description, int min, int max) : base(hasVariable, identifier, description, min, max)
        {
        }

        #region Overrides of ApplicationIdentifier

        public override DataFormatType DataFormat { get; protected set; } = DataFormatType.Alphanumeric;
        public override object Parse()
        {
            // TODO: Validate with regex
            return ElementData;
        }

        #endregion
    }
}