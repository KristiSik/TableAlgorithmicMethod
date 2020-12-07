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

        public int GetExponent(int a)
        {
            return 0;
        }

        public int GetMantissa(int a)
        {
            switch (FixedPointNumberFormat)
            {
                case FixedPointNumberFormat.Q15:
                    return a & 0x7FFF;

                case FixedPointNumberFormat.Q23:
                    return a & 0x7FFFFF;

                default:
                    throw new Exception($"Format {FixedPointNumberFormat} is not supported");
            }
        }

        public int MantissaRigthShift(int a, int shift)
        {
            switch (FixedPointNumberFormat)
            {
                case FixedPointNumberFormat.Q15:
                    return ((a & 0x7FFF) >> shift) | (a & 8000);

                case FixedPointNumberFormat.Q23:
                    return ((a & 0x7FFFFF) >> shift) | (a & 80000);

                default:
                    throw new Exception($"Format {FixedPointNumberFormat} is not supported");
            }
        }

        public double Error(int a, int b)
        {
            double decimalA = 0;
            double decimalB = 0;

            for (int i = 1, j = (int)FixedPointNumberFormat - 1; j >= 0; i++, j--)
            {
                decimalA += ((a >> j) & 0x1) * Math.Pow(2, -i);
                decimalB += ((b >> j) & 0x1) * Math.Pow(2, -i);
            }

            return Math.Abs(decimalA - decimalB);
        }
    }
}
