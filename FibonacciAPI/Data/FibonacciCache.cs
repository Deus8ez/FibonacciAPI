using Microsoft.Extensions.Caching.Memory;
using FibonacciAPI.Interfaces;

namespace FibonacciAPI.Data
{
    public class FibonacciCache : IFibonacciCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;

        public FibonacciCache(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            this.configuration = configuration; 
        }

        public int[] GetArrayWithLongestSequence()
        {
            int[] arr;
            memoryCache.TryGetValue(GetLongestSequenceLength(), out arr);
            return arr;
        }

        public void Cache(int[] arr)
        {
            var cachingParams = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(configuration.GetValue<int>("CacheExpirationMinutes")));
            if (GetLongestSequenceLength() < arr.Length)
            {
                memoryCache.Set("longestSequenceLength", arr.Length, cachingParams);
                memoryCache.Set(arr.Length, arr, cachingParams);
            }
        }

        private int GetLongestSequenceLength()
        {
            int longestSequenceLength;
            memoryCache.TryGetValue("longestSequenceLength", out longestSequenceLength);
            return longestSequenceLength;
        }
    }
}
