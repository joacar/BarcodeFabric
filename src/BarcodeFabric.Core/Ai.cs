using System;
using System.Linq;

namespace BarcodeFabric.Core
{
    /// <summary>
    /// Struct to handle application idenfier (AI) when parsing barcode.
    /// </summary>
    /// <remarks>
    /// The function <see cref="CalculateKey"/> calculates the <see cref="ApplicationIdentiferType"/> value that will be used when finding the 
    /// <see cref="ApplicationIdentifier"/>.
    /// </remarks>
    public struct Ai
    {
        /// <summary>
        /// Initialize a <see cref="Ai"/> with value <paramref name="identifiers"/>
        /// </summary>
        /// <param name="identifiers">List of <see langword="chars" /> that comprise the AI</param>
        public Ai(params char[] identifiers)
        {
            Identifier = identifiers;
        }

        private char[] Identifier { get; }

        public bool HasVariable
            => Identifier.First() == '3' && Identifier.Length == 4 && Identifier.Last() != default(char);

        /// <summary>
        /// Get the value of the variable character
        /// </summary>
        public int Variable => (int)char.GetNumericValue(Identifier.Last());

        /// <summary>
        /// Creates the string representation by calling <code>new string(<see cref="Identifier"/>)</code>
        /// </summary>
        /// <returns>The <see cref="string"/> value for chars in <see cref="Identifier"/></returns>
        public override string ToString()
        {
            return new string(Identifier);
        }

        public static explicit operator Ai(string value)
        {
            return new Ai(value.ToCharArray());
        }

        public static explicit operator Ai(char[] value)
        {
            return new Ai(value);
        }

        public static implicit operator string(Ai value)
        {
            return value.ToString();
        }

        public static implicit operator ushort(Ai value)
        {
            return value.CalculateKey();
        }

        /// <summary>
        /// Calculate the numeric value based on <see cref="Identifier"/>
        /// </summary>
        /// <returns></returns>
        public ushort CalculateKey()
        {
            var identifiers = Identifier.Where(i => i != default(char)).ToList();
            var exponent = identifiers.Count - 1;
            if (HasVariable)
            {
                // Application identifiers that start with three (3) and are of length four (4)
                // are parsed when reading data from barcode and to match with identifier the
                // variable value should not be parsed
                // See http://www.gs1-128.info/application-identifiers/
                --exponent;
            }
            double key = 0;
            foreach (var identifier in identifiers)
            {
                key += Math.Pow(10, exponent) * char.GetNumericValue(identifier);
                --exponent;
                if (exponent < 0)
                {
                    break;
                }
            }
            return (ushort)key;
        }
    }
}