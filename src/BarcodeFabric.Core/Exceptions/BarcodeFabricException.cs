using System;

namespace BarcodeFabric.Core
{
    public class BarcodeFabricException : Exception
    {
        public BarcodeFabricException(string message, Exception innerException) : base(message, innerException)
        {

        }
        public BarcodeFabricException(string message) : base(message)
        {

        }
    }
}