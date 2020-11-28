namespace TableAlgorithmicMethod.Models
{
    public class ScalarMultiplicationResult
    {
        public ScalarMultiplicationResult(int value, long elapsedTicks)
        {
            Value = value;
            ElapsedTicks = elapsedTicks;
        }

        public int Value { get; }

        public long ElapsedTicks { get; }
    }
}
