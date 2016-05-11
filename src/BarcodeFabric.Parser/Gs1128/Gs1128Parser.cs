using System;
using System.Text;
using BarcodeFabric.Core;

namespace BarcodeFabric.Parser
{
    public class Gs1128Parser : ParserBase
    {
        private const int MaxApplicationIdentifierLength = 4;
        public static readonly char Fnc1 = Convert.ToChar(29);
        private readonly char[] _ai;


        private readonly StringBuilder _stringBuilder;


        public Gs1128Parser(string data) : base(data)
        {
            _ai = new char[MaxApplicationIdentifierLength];
            _stringBuilder = new StringBuilder(data.Length);
        }

        public override Barcode Parse()
        {
            while (Tokenizer.CanRead())
            {
                // Parse application identifier
                var identifier = ParseApplicationIdentifier();
                // Parse element data for the identifier
                ParseApplicationIdentifierData(identifier);
                Barcode.Add(identifier.Identifier, identifier);
            }
            // TODO: Sanity check that we read all input?
            return Barcode;
        }

        protected ApplicationIdentifier ParseApplicationIdentifier()
        {
            var start = Tokenizer.Position;
            // An application identifier is at least two digits and at most four digits
            var ai = Create(0, start, 2);
            if (!ApplicationIdentifierManager.Missing(ai))
            {
                return ApplicationIdentifierManager.Get(ai);
            }
            ai = Create(2, start + 2, 2);
            if (ApplicationIdentifierManager.Missing(ai))
            {
                throw new InvalidApplicationIdentifierException($"Application identifier '{ai}' at position {start} was not found");
            }
            var value = ApplicationIdentifierManager.Get(ai);
            if (ai.HasVariable)
            {
                value.Variable = ai.Variable;
            }
            return value;
        }

        private AI Create(int targetStart, int sourceStart, int length)
        {
            Tokenizer.Take(_ai, targetStart, sourceStart, length);
            var ai = (AI)_ai;
            return ai;
        }

        protected void ParseApplicationIdentifierData(ApplicationIdentifier applicationIdentifier)
        {
            if (applicationIdentifier.IsFixed)
            {
                var chars = new char[applicationIdentifier.Max];
                Tokenizer.Take(chars, 0, chars.Length);
                // TODO: Delay this?
                applicationIdentifier.ElementData = new string(chars);
                return;
            }
            var start = Tokenizer.Position;
            // Application identifier data is of variable length and is delimited by FNC1
            while (Tokenizer.CanRead())
            {
                var c = Tokenizer.Pop();
                // Parse to functional code one (fnc1) or start of next application data element
                if (c == Fnc1)
                {
                    break;
                }
                _stringBuilder.Append(c);
            }
            // TODO: How to determine if we reached end of input correctly or incorrectly?
            if (!Tokenizer.CanRead() && Barcode.Count > 0)
            {
                // We read to the end of input. Most likely variable length data 
                // that was not delimited by FNC1
                //throw new InvalidTokenException($"Data '{new string(_tokenizer.Chars)}' is missing FNC1 character for application identifier '{applicationIdentifier.Identifier}' ending at position {start - 1}");
            }
            applicationIdentifier.ElementData = _stringBuilder.ToString();
            _stringBuilder.Clear();
        }
    }
}