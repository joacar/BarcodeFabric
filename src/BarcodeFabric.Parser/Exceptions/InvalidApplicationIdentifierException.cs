using System;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public class InvalidApplicationIdentifierException : BarcodeFabricException
    {
        public InvalidApplicationIdentifierException(string message) : base(message) { }

        public InvalidApplicationIdentifierException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}