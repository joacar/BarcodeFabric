using System;
using System.Globalization;
using BarcodeFabric.Core.Exceptions;

namespace BarcodeFabric.Core
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
            catch
            {
                throw new InvalidElementDataException($"Element data '{ElementData}' must be of form 'yyMMdd'");

            }
        }

        #endregion
    }
}