using System;
using System.Globalization;

namespace BarcodeFabric.Parser
{
    public class DateApplicationIdentifier : ApplicationIdentifier
    {
        public DateApplicationIdentifier(string identifier, string description, int min, int max) : base(identifier, description, min, max)
        {
        }

        public DateApplicationIdentifier(bool hasVariable, string identifier, string description, int min, int max) : base(hasVariable, identifier, description, min, max)
        {
        }

        #region Overrides of ApplicationIdentifier

        public override DataFormatType DataFormat { get; protected set; } = DataFormatType.Date;
        public override object Parse()
        {
            // TODO: Add class to support hours and minutes
            try
            {
                return DateTime.ParseExact(ElementData, "yyMMdd", DateTimeFormatInfo.InvariantInfo).Date;
            }
            catch (Exception exception)
            {
                throw new InvalidElementDataException($"Element data '{ElementData}' must be of form 'yyMMdd'", exception);

            }
        }

        #endregion
    }
}