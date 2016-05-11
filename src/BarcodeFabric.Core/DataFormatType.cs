namespace BarcodeFabric.Core
{
    /// <summary>
    /// Data formats for the element data part for an application identifier
    /// </summary>
    public enum DataFormatType
    {
        /// <summary>
        /// Element data is alphanumeric
        /// </summary>
        Alphanumeric,
        /// <summary>
        /// Element data is numeric
        /// </summary>
        Numeric,
        /// <summary>
        /// Element data is digits
        /// </summary>
        Digit,
        /// <summary>
        /// Element data is date
        /// </summary>
        Date,
        /// <summary>
        /// Element data is date and time
        /// </summary>
        DateTime
    }
}