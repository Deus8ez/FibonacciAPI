using Microsoft.Extensions.Caching.Memory;
using FibonacciAPI.Interfaces;
using FibonacciAPI.Helpers;

namespace FibonacciAPI.Services
{
    public class FibonacciService : IFibonacciService
    {
        private readonly IFibonacciCache fibonacciCache;

        public FibonacciService(IFibonacciCache fibonacciCache)
        {
            this.fibonacciCache = fibonacciCache;
        }

        public int[] GetSequence(int start, int end, bool fromCache)
        {
            int[] res;

            if (fromCache)
            {
                var cachedSequence = fibonacciCache.GetArrayWithLongestSequence();
                res = FibonacciGenerator.Generate(end, cachedSequence);
            } else
            {
                res = FibonacciGenerator.Generate(end);
            }

            fibonacciCache.Cache(res);

            return res[start..end];
        }
    }
}
