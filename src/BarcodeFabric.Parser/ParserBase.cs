using System;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public abstract class ParserBase
    {
        protected ParserBase(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Data can not be null nor empty", nameof(data));
            }
            Tokenizer = new Tokenizer(data);
            Barcode = new Barcode(data);
        }

        protected Tokenizer Tokenizer { get; }
        protected Barcode Barcode { get; }

        public abstract Barcode Parse();
    }
}