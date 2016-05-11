using System.Collections.Generic;

namespace BarcodeFabric.Core
{
    public class Barcode : Dictionary<string, object>
    {
        public Barcode(string data)
        {
            Data = data;
        }

        /// <summary>
        /// Original data string
        /// </summary>
        public string Data { get; }
    }
}