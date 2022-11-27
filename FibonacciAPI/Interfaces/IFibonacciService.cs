namespace FibonacciAPI.Interfaces
{
    public interface IFibonacciService
    {
        public int[] GetSequence(int start, int end, bool cached);
    }
}
