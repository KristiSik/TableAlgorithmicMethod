namespace TableAlgorithmicMethod.Models
{
    public abstract class Number
    {
        public int RawValue { get; protected set; }

        protected abstract int Add(Number n);

        protected abstract Number Multiply(Number n);

        public static int operator +(Number n1, Number n2)
        {
            return n1.Add(n2);
        }

        public static Number operator *(Number n1, Number n2)
        {
            return n1.Multiply(n2);
        }
    }
}
