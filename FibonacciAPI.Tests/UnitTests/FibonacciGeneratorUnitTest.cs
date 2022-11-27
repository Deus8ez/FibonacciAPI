using FibonacciAPI.Helpers;

namespace FibonacciAPI.Tests.UnitTests
{
    public class FibonacciGeneratorUnitTest
    {
        [Fact]
        public void ReturnsCorrectSequence()
        {
            Assert.Equal(new int[] { 0, 1, 1 }, FibonacciGenerator.Generate(2));
            Assert.Equal(new int[] { 0, 1, 1, 2, 3, 5 }, FibonacciGenerator.Generate(5));
            Assert.Equal(new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 }, FibonacciGenerator.Generate(9));
            Assert.Equal(new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233 }, FibonacciGenerator.Generate(13));
        }
    }
}