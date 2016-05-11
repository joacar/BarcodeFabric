using System;
using System.Collections.Generic;

namespace BarcodeFabric.Core
{
    public enum ApplicationIdentiferType : ushort
    {
        Sscc = 0,
        Gtin = 1,

        BatchNumber = 10,
        ProductionDate = 11,
        ExpirationDate = 17,

        SerialNumber = 21,

        ProductionNetWeight = 301
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

        private static readonly IDictionary<ushort, Lazy<ApplicationIdentifier>> Identifiers =
            new Dictionary<ushort, Lazy<ApplicationIdentifier>>();

        static ApplicationIdentifierManager()
        {
            Init();
        }

        private static void Init()
        {
            AddApplicationIdentifier(ApplicationIdentiferType.Sscc,
                () => new NumericApplicationIdentifier(Sscc, "Serial Shipping Container Code 18 (SSCC)", 18, 18));
            AddApplicationIdentifier(ApplicationIdentiferType.Gtin,
                () => new NumericApplicationIdentifier(Gtin, "Global Trade Item Number (GTIN)", 14, 14));
            AddApplicationIdentifier(ApplicationIdentiferType.ProductionDate,
                () => new DateApplicationIdentifier(ProductionDate, "Production Date", 6, 6));
            AddApplicationIdentifier(ApplicationIdentiferType.ExpirationDate,
                () => new DateApplicationIdentifier(ExpirationDate, "Expiration Date", 6, 6));
            AddApplicationIdentifier(ApplicationIdentiferType.BatchNumber,
                () => new AlphanumericApplicationIdentifier(BatchNumber, "Batch Number", 1, 20));
            AddApplicationIdentifier(ApplicationIdentiferType.SerialNumber,
                () => new AlphanumericApplicationIdentifier(SerialNumber, "Serial Number", 1, 20));
            AddApplicationIdentifier(ApplicationIdentiferType.ProductionNetWeight,
                () => new DigitApplicationIdentifier(true, ProductionNetWeight, "Production Net Weight", 6, 6));
        }

        /// <summary>
        /// Add the <paramref name="applicationIdentifier" /> to list of known application identifiers
        /// </summary>
        /// <remarks>
        /// When added an instance of <see cref="Ai" /> will be created by calling
        /// <code>applicationIdentifier.Identifier.ToCharArray()</code>
        /// </remarks>
        /// <param name="applicationIdentifier">The <see cref="ApplicationIdentifier" /> to add</param>
        /// <returns><c>true</c> iff not exists; otherwise <c>false</c></returns>
        public static bool AddApplicationIdentifier(ApplicationIdentifier applicationIdentifier)
        {
            var ai = (Ai)applicationIdentifier.Identifier.ToCharArray();
            if (Identifiers.ContainsKey(ai))
            {
                return false;
            }
            Identifiers.Add(ai, new Lazy<ApplicationIdentifier>(() => applicationIdentifier));
            return true;
        }

        internal static bool AddApplicationIdentifier(ApplicationIdentiferType key,
            Func<ApplicationIdentifier> applicationIdentifier)
        {
            return AddApplicationIdentifier((ushort)key, applicationIdentifier);
        }

        public static bool AddApplicationIdentifier(ushort key, Func<ApplicationIdentifier> applicationIdentifier)
        {
            if (Identifiers.ContainsKey(key))
            {
                return false;
            }
            Identifiers.Add(key, new Lazy<ApplicationIdentifier>(applicationIdentifier));
            return true;
        }

        public static bool Missing(Ai identifier)
        {
            return !Identifiers.ContainsKey(identifier);
        }

        public static ApplicationIdentifier Get(Ai identifier)
        {
            return Identifiers[identifier].Value;
        }

        public static bool Missing(ushort identifier)
        {
            return !Identifiers.ContainsKey(identifier);
        }

        public static ApplicationIdentifier Get(ushort identifier)
        {
            return Identifiers[identifier].Value;
        }
    }
}