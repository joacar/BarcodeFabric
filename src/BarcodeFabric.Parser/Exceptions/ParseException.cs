using System;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public class ParseException : BarcodeFabricException
    {
        public ParseException(string message) : base(message)
        {
        }

        public ParseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}