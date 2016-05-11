using System;
using System.Collections.Generic;

namespace BarcodeFabric.Parser
{
    public enum ApplicationIdentifierType
    {
        Sscc = 0,
        Gtin = 1,
        GtinContent = 2,
        BatchNumber = 10,
        ProductionDate = 11,
        DueDate = 12,
        PackagingDate = 13,
        BestBeforeDate = 15,
        SellByDate = 16,
        ExpirationDate = 17,

        SerialNumber = 21,

        ProductionNetWeight = 301,
    }

    public static class ApplicationIdentifierManager
    {
        /// <summary>
        /// Serial Shipping Container Code (00)
        /// </summary>
        public static readonly string Sscc = "00";

        /// <summary>
        /// Global Trade Item Number (01)
        /// </summary>
        public static readonly string Gtin = "01";

        /// <summary>
        /// Gtin of contained trade items (02)
        /// </summary>
        public static readonly string GtinContent = "02";

        /// <summary>
        /// Production Date (11)
        /// </summary>
        public static readonly string ProductionDate = "11";

        /// <summary>
        /// Expiration Date (17)
        /// </summary>
        public static readonly string ExpirationDate = "17";

        /// <summary>
        /// Batch Number (10)
        /// </summary>
        public static readonly string BatchNumber = "10";

        /// <summary>
        /// Serial Number (21)
        /// </summary>
        public static readonly string SerialNumber = "21";

        /// <summary>
        /// Production Net Weight (301)
        /// </summary>
        public static readonly string ProductionNetWeight = "301";

        private static readonly IDictionary<int, Lazy<ApplicationIdentifier>> Identifiers =
            new Dictionary<int, Lazy<ApplicationIdentifier>>();

        static ApplicationIdentifierManager()
        {
            AddApplicationIdentifier(ApplicationIdentifierType.Sscc,
                () => new NumericApplicationIdentifier(Sscc, "Serial Shipping Container Code", 18, 18));
            AddApplicationIdentifier(ApplicationIdentifierType.Gtin,
                () => new NumericApplicationIdentifier(Gtin, "Global Trade Item Number", 14, 14));
            AddApplicationIdentifier(ApplicationIdentifierType.GtinContent,
                () => new NumericApplicationIdentifier(GtinContent, "GTIN of contained trade items", 14, 14));
            AddApplicationIdentifier(ApplicationIdentifierType.ProductionDate,
                () => new DateApplicationIdentifier(ProductionDate, "Production ate", 6, 6));
            AddApplicationIdentifier(ApplicationIdentifierType.ExpirationDate,
                () => new DateApplicationIdentifier(ExpirationDate, "Expiration Date", 6, 6));
            AddApplicationIdentifier(ApplicationIdentifierType.BatchNumber,
                () => new AlphanumericApplicationIdentifier(BatchNumber, "Batch Number", 1, 20));
            AddApplicationIdentifier(ApplicationIdentifierType.SerialNumber,
                () => new AlphanumericApplicationIdentifier(SerialNumber, "Serial Number", 1, 20));
            AddApplicationIdentifier(ApplicationIdentifierType.ProductionNetWeight,
                () => new DigitApplicationIdentifier(true, ProductionNetWeight, "Production Net Weight", 6, 6));
        }

        /// <summary>
        /// Add the <paramref name="applicationIdentifier" /> to list of known application identifiers identified by <paramref name="key"/>
        /// </summary>
        /// <remarks>
        /// When added an instance of <see cref="AI" /> will be created by calling
        /// <code>applicationIdentifier.Identifier.ToCharArray()</code>
        /// </remarks>
        /// <param name="key">Application identifier key</param>
        /// <param name="applicationIdentifier">The <see cref="ApplicationIdentifier" /> to add</param>
        /// <returns><c>true</c> iff not exists; otherwise <c>false</c></returns>
        internal static bool AddApplicationIdentifier(ApplicationIdentifierType key,
            Func<ApplicationIdentifier> applicationIdentifier)
        {
            return AddApplicationIdentifier((int)key, applicationIdentifier);
        }

        public static bool AddApplicationIdentifier(int key, Func<ApplicationIdentifier> applicationIdentifier)
        {
            if (Identifiers.ContainsKey(key))
            {
                return false;
            }
            Identifiers.Add(key, new Lazy<ApplicationIdentifier>(applicationIdentifier));
            return true;
        }

        public static bool Missing(AI identifier)
        {
            return !Identifiers.ContainsKey(identifier);
        }

        public static ApplicationIdentifier Get(AI identifier)
        {
            return Identifiers[identifier].Value;
        }

        public static bool Missing(int identifier)
        {
            return !Identifiers.ContainsKey(identifier);
        }

        public static ApplicationIdentifier Get(int identifier)
        {
            return Identifiers[identifier].Value;
        }
    }
}