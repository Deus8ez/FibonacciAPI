namespace FibonacciAPI.Interfaces
{
    public interface IFibonacciCache
    {
        public int[] GetArrayWithLongestSequence();
        public void Cache(int[] arr);
    }
}
