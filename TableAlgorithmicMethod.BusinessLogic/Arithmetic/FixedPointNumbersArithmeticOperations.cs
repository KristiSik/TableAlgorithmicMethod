using System;

namespace TableAlgorithmicMethod.BusinessLogic.Arithmetic
{
    public class FixedPointNumbersArithmeticOperations : IArithmeticOperations
    {
        public FixedPointNumbersArithmeticOperations(FixedPointNumberFormat fixedPointNumberFormat)
        {
            FixedPointNumberFormat = fixedPointNumberFormat;
            NumberSize = (int)fixedPointNumberFormat + 1;
        }

        public FixedPointNumberFormat FixedPointNumberFormat { get; }

        public int NumberSize { get; }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            switch (FixedPointNumberFormat)
            {
                case FixedPointNumberFormat.Q15:
                    return (int)((uint)(((long)(a & 0x7FFF) * (b & 0x7FFF)) >> 15) | (uint)((a ^ b) & 0x8000));

                case FixedPointNumberFormat.Q23:
                    return (int)((uint)(((long)(a & 0x7FFFFF) * (b & 0x7FFFFF)) >> 23) | (uint)((a ^ b) & 0x800000));

                default:
                    throw new Exception($"Format {FixedPointNumberFormat} is not supported");
            }
        }
    }
}
