using System;
using System.Text.RegularExpressions;

namespace BarcodeFabric.Core
{
    public static class ElementDataValidator
    {
        private const string NumericPattern = "[0-9]{{{0},{1}}}";
        private const string AlphanumericPattern = "[a-zA-Z0-9]{{{0},{1}}}";

        public static bool Validate(DataFormatType dataFormat, int length, string data)
        {
            return Validate(dataFormat, length, length, data);
        }

        public static bool Validate(DataFormatType dataFormat, int min, int max, string data)
        {
            switch (dataFormat)
            {
                case DataFormatType.Alphanumeric:
                    return Regex.IsMatch(data, string.Format(AlphanumericPattern, min, max));
                case DataFormatType.Numeric:
                    return Regex.IsMatch(data, string.Format(NumericPattern, min, max));
                case DataFormatType.Digit:
                    return Regex.IsMatch(data, string.Format(NumericPattern, min, max));
                case DataFormatType.Date:
                    break;
                case DataFormatType.DateTime:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataFormat), dataFormat, null);
            }
            throw new ArgumentOutOfRangeException(nameof(dataFormat), dataFormat, null);
        }
    }
}
