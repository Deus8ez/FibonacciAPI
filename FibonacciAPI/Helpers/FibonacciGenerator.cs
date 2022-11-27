namespace FibonacciAPI.Helpers
{
    public static class FibonacciGenerator
    {
        public static int[] Generate(int n, int[]? cachedSequence = null)
        {
            var arr = new int[n + 1];
            arr[0] = 0;
            arr[1] = 1;

            //index starting sequence generation
            int i = 2;

            if (cachedSequence != null)
            {
                while (i < arr.Length && i < cachedSequence.Length)
                {
                    arr[i] = cachedSequence[i];
                    i++;
                }
            }

            while (i < arr.Length)
            {
                arr[i] = arr[i - 1] + arr[i - 2];
                i++;
            }

            return arr;
        }
    }
}
