using System;
using System.Linq;

namespace BarcodeFabric.Parser
{
    /// <summary>
    /// Struct to handle application idenfier (AI) when parsing barcode.
    /// </summary>
    /// <remarks>
    /// The function <see cref="CalculateKey"/> calculates the <see cref="ApplicationIdentifierType"/> value that will be used when finding the 
    /// <see cref="ApplicationIdentifier"/>.
    /// </remarks>
    public struct AI : IEquatable<AI>
    {
        #region Equality members

        public bool Equals(AI other)
        {
            return CalculateKey() == other.CalculateKey();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AI && Equals((AI)obj);
        }

        public override int GetHashCode()
        {
            return CalculateKey();
        }

        public static bool operator ==(AI left, AI right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AI left, AI right)
        {
            return !left.Equals(right);
        }

        #endregion

        /// <summary>
        /// Initialize a <see cref="AI"/> with value <paramref name="identifiers"/>
        /// </summary>
        /// <param name="identifiers">List of <see langword="chars" /> that comprise the AI</param>
        public AI(params char[] identifiers)
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

        public static explicit operator AI(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 4)
            {
                throw new ArgumentException("string must contain at least two characters and at most four", nameof(value));
            }
            return new AI(value.ToCharArray());
        }

        public static explicit operator AI(char[] value)
        {
            return new AI(value);
        }

        public static implicit operator string(AI value)
        {
            return value.ToString();
        }

        public static implicit operator int(AI value)
        {
            return value.CalculateKey();
        }

        /// <summary>
        /// Calculate the numeric value based on <see cref="Identifier"/>
        /// </summary>
        /// <returns></returns>
        public int CalculateKey()
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
            return (int)key;
        }
    }
}