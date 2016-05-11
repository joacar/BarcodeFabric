using Xunit;

namespace BarcodeFabric.Parser.Tests
{
    public class AiFixture
    {
        [Theory]
        [InlineData(0, new[] { '0', '0' })]
        [InlineData(1, new[] { '0', '1' })]
        [InlineData(21, new[] { '2', '1' })]
        [InlineData(301, new[] { '3', '0', '1' })]
        [InlineData(301, new[] { '3', '0', '1', '1' })]
        [InlineData(8070, new[] { '8', '0', '7', '0' })]
        public void CalculateKey(ushort key, char[] chars)
        {
            var ai = new AI(chars);
            Assert.Equal(key, ai.CalculateKey());
        }

        [Fact]
        public void Ai_HasVariable_False()
        {
            var ai = new AI('0', '0');
            Assert.False(ai.HasVariable);
        }

        [Fact]
        public void Ai_HasVariable_False_LengthFour()
        {
            var ai = new AI('8', '0', '7', '0');
            Assert.False(ai.HasVariable);
        }

        [Fact]
        public void Ai_HasVariable_False_StartsWithThree()
        {
            var ai = new AI('3', '0', '1');
            Assert.False(ai.HasVariable);
        }

        [Fact]
        public void Ai_HasVariable_True()
        {
            var ai = new AI('3', '0', '1', '3');
            Assert.True(ai.HasVariable);
        }
    }
}