using System;
using System.Text;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public class GtinParser : ParserBase
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public GtinParser(string code) : base(code)
        {
        }

        public override Barcode Parse()
        {
            var first = Tokenizer.Pop();
            var second = Tokenizer.Pop();
            // Check gs1 weight item prefix code that encode either weight or price data
            if (first == '2')
            {
                int exponent;
                switch (second)
                {
                    case '0':
                        exponent = 2;
                        Barcode["price"] = true;
                        break;
                    case '1':
                        exponent = 1;
                        Barcode["price"] = true;
                        break;
                    case '2':
                        exponent = 0;
                        Barcode["price"] = true;
                        break;
                    case '3':
                        exponent = 3;
                        Barcode["weight"] = true;
                        break;
                    case '4':
                        exponent = 2;
                        Barcode["weight"] = true;
                        break;
                    case '5':
                        exponent = 1;
                        Barcode["weight"] = true;
                        break;
                    default:
                        goto end;
                }
                ConstructGtin(first, second, exponent);
            }
            end:
            return Barcode;
        }

        private int DecodeData(int exponent)
        {
            // Parse encoded data at position [8,11]
            double data = 0;
            for (int index = 8, exp = 3; index < 12; ++index, --exp)
            {
                data += Math.Pow(10, exp)*char.GetNumericValue(Tokenizer.Pop());
            }
            data /= Math.Pow(10, exponent);
            Barcode["wpd"] = data;
            return 4;
        }

        private void ConstructGtin(char first, char second, int exponent)
        {
            // Construct gtin
            _stringBuilder.Append($"{first}{second}");
            for (var index = 2; index < 8; ++index)
            {
                _stringBuilder.Append(Tokenizer.Pop());
            }
            var zeros = DecodeData(exponent);
            _stringBuilder.Append('0', zeros);
            if (Tokenizer.CanRead())
            {
                // Check digit
                _stringBuilder.Append(Tokenizer.Pop());
            }
            Barcode["gtin"] = _stringBuilder.ToString();
        }
    }
}