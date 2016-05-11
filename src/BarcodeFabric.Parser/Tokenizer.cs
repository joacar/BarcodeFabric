using System;
using System.Diagnostics;

namespace BarcodeFabric.Parser
{
    [DebuggerDisplay("Tokenzing {new string(_data)}' ({_data.Length}). Cursor at {Position}")]
    public class Tokenizer
    {
        private readonly char[] _data;

        public Tokenizer(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Data must contain more than zero digits", nameof(data));
            }
            _data = data.ToCharArray();
        }

        public Tokenizer(char[] data)
        {
            if (data.Length == 0)
            {
                throw new ArgumentException("Data must contain more than zero elements", nameof(data));
            }
            _data = data;
        }

        public int Position { get; private set; }

        /// <summary>
        /// Check that parser has not reached end of input
        /// </summary>
        /// <returns></returns>
        public bool CanRead()
        {
            return Position < _data.Length;
        }

        /// <summary>
        /// Peek at the current char in <see cref="_data" />
        /// </summary>
        /// <returns><see langword="char" /> at index <see cref="Position" /> in <see cref="_data" /></returns>
        public char Peek()
        {
            CheckIndexConstraint();
            return _data[Position];
        }

        /// <summary>
        /// Pop the next char in <see cref="_data" /> and increment <see cref="Position" /> by one
        /// </summary>
        /// <returns></returns>
        public char Pop()
        {
            CheckIndexConstraint();
            var c = _data[Position];
            ++Position;
            return c;
        }

        private void CheckIndexConstraint()
        {
            if (Position > _data.Length)
            {
                throw new ParseException($"Cannot move curstor to position {Position} since available data is {_data.Length}");
            }
        }

        /// <summary>
        /// Move the cursor <paramref name="count" /> steps.
        /// </summary>
        /// <param name="count">Number of tokens to skip</param>
        /// <returns>Skipped tokens</returns>
        public int Pop(int count)
        {
            Position += count;
            CheckIndexConstraint();
            return count;
        }

        public void Take(char[] target, int targetStart, int length)
        {
            Take(target, targetStart, Position, length);
        }

        public void Take(char[] target, int targetStart, int sourceStart, int length)
        {
            if (sourceStart + length >= _data.Length)
            {
                throw new ParseException($"Cannot copy {length} bytes starting from {sourceStart} since available data is {_data.Length}");
            }
            Array.Copy(_data, sourceStart, target, targetStart, length);
            // Move cursor forward
            Pop(length);
        }
    }
}