using System;

namespace TableAlgorithmicMethod.BusinessLogic.Arithmetic
{
    public class FloatingPointNumbersArithmeticOperations : IArithmeticOperations
    {
        public const int FLOATING_POINT_NUMBER_SIZE = 32;
        public const uint EXPONENT_SIGN_MASK = 0x80000000;
        public const uint EXPONENT_MASK = 0x7F000000;
        public const int EXPONENT_ONE = 0x1000000;
        public const uint MANTISSA_MASK = 0xFFFFFF;
        public const uint MANTISSA_FIRST_BIT_MASK = 0x800000;

        public int NumberSize => FLOATING_POINT_NUMBER_SIZE;

        public int Add(int a, int b)
        {
            int exponentA = (int)(a & EXPONENT_MASK);
            int exponentB = (int)(b & EXPONENT_MASK);
            int mantissaA = (int)(a & MANTISSA_MASK);
            int mantissaB = (int)(b & MANTISSA_MASK);

            while (exponentA > exponentB)
            {
                exponentB += EXPONENT_ONE;
                mantissaB >>= 1;
            }

            while (exponentB > exponentA)
            {
                exponentA += EXPONENT_ONE;
                mantissaA >>= 1;
            }

            int exponent = exponentA;
            int mantissa = mantissaA + mantissaB;
            NormalizeManissa(ref mantissa, ref exponent);

            return exponent | mantissa;
        }

        public int GetExponent(int a)
        {
            return (int)((a & EXPONENT_MASK) >> 24);
        }

        public int GetMantissa(int a)
        {
            return (int)(a & MANTISSA_MASK);
        }

        public int Multiply(int a, int b)
        {
            int mantissa = (int)((((a & MANTISSA_MASK) * (b & MANTISSA_MASK)) >> 24) & MANTISSA_MASK);
            int exponent = (int)(a & EXPONENT_MASK) + (int)(b & EXPONENT_MASK);
            NormalizeManissa(ref mantissa, ref exponent);
            return (int)((a & EXPONENT_SIGN_MASK) ^ (b & EXPONENT_SIGN_MASK)) | exponent | mantissa;
        }

        public static void NormalizeManissa(ref int mantissa, ref int exponent)
        {
            while (mantissa != 0 && (mantissa & EXPONENT_MASK) > 0)
            {
                mantissa >>= 1;
                exponent += EXPONENT_ONE;
            }

            while (mantissa != 0 && (mantissa & MANTISSA_FIRST_BIT_MASK) == 0 && (exponent >= MANTISSA_FIRST_BIT_MASK))
            {
                mantissa <<= 1;
                exponent -= EXPONENT_ONE;
            }
        }

        public int MantissaRigthShift(int a, int shift)
        {
            int mantissa = (int)((a & MANTISSA_MASK) >> shift);
            int exponent = (int)(a & EXPONENT_MASK);
            for (int i = 0; i < shift; i++)
            {
                exponent += EXPONENT_ONE;
            }

            return exponent | mantissa;
        }

        private double ToDecimal(int a)
        {
            int exponentDecimal = 0;
            double mantissaDecimal = 0;

            for (int i = 24, j = 0; i < 31; i++, j++)
            {
                exponentDecimal += ((a >> i) & 0x1) * (int)Math.Pow(2, j);
            }

            for (int i = 23, j = 0; i >= 0; i--, j++)
            {
                mantissaDecimal += ((a >> i) & 0x1) * Math.Pow(2, -j);
            }

            return mantissaDecimal * Math.Pow(2, exponentDecimal);
        }

        public double Error(int a, int b)
        {
            return Math.Abs(ToDecimal(a) - ToDecimal(b));
        }
    }
}
