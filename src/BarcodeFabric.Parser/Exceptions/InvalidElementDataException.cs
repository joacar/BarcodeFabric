using System;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public class InvalidElementDataException : BarcodeFabricException
    {
        public InvalidElementDataException(string message) : base(message)
        {
        }

        public InvalidElementDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}