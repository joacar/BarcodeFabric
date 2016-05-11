using System;
using System.Diagnostics;

namespace BarcodeFabric.Parser
{
    /// <summary>
    /// Struct encapsulating application identifier data such as identifier, description and data format
    /// </summary>
    /// <remarks>
    /// The data format is represented as a <see cref="DataFormat" /> object
    /// </remarks>
    [DebuggerDisplay("{Identifier} ({Description}). Element data '{ElementData}'")]
    public abstract class ApplicationIdentifier
    {
        private int? _variable;

        protected ApplicationIdentifier(string identifier, string description, int min, int max)
            : this(false, identifier, description, min, max)
        {
        }

        protected ApplicationIdentifier(bool hasVariable, string identifier, string description, int min, int max)

        {
            Identifier = identifier;
            Description = description;
            HasVariable = hasVariable;
            Min = min;
            Max = max;
        }

        public bool IsFixed => Min == Max;

        public int Min { get; }

        public int Max { get; }

        /// <summary>
        /// Raw data that was parsed when decoding barcode data
        /// </summary>
        public string ElementData { get; set; }

        /// <summary>
        /// Data format for the application identifier
        /// </summary>
        public abstract DataFormatType DataFormat { get; protected set; }

        public int Variable
        {
            get
            {
                if (!_variable.HasValue)
                {
                    throw new InvalidOperationException(
                        "Identifier does not contain a variable character or it has not been set");
                }
                return _variable.Value;
            }
            set { _variable = value; }
        }

        public bool HasVariable { get; }

        /// <summary>
        /// Application identifier
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Description for the application identifier
        /// </summary>
        public string Description { get; }

        public abstract object Parse();
    }
}