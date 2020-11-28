using System;
using System.Text;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.Helpers
{
    public static class BinaryOperations
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Multiply(int a, int b, FixedPointNumberFormat fixedPointNumberFormat)
        {
            switch (fixedPointNumberFormat)
            {
                case FixedPointNumberFormat.Q15:
                    return (int)((uint)(((long)(a & 0x7FFF) * (b & 0x7FFF)) >> 15) | (uint)((a ^ b) & 0x8000));

                case FixedPointNumberFormat.Q23:
                    return (int)((uint)(((long)(a & 0x7FFFFF) * (b & 0x7FFFFF)) >> 23) | (uint)((a ^ b) & 0x800000));

                default:
                    throw new Exception($"Format {fixedPointNumberFormat} is not supported");
            }
        }

        public static int Parse(string binaryStr, int startIndex)
        {
            int result = 0;
            if (string.IsNullOrEmpty(binaryStr))
            {
                throw new ArgumentException($"'{nameof(binaryStr)}' cannot be null or empty", nameof(binaryStr));
            }

            for (int i = 0, j = startIndex; j >= 0 & i < binaryStr.Length; i++, j--)
            {
                switch (binaryStr[i])
                {
                    case '0':
                        break;

                    case '1':
                        result |= 1 << j;
                        break;

                    default:
                        throw new Exception($"Unexpected character '{binaryStr[i]}'");
                }
            }

            return result;
        }

        public static string ToString(int value, int numberOfDigits)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numberOfDigits; i++)
            {
                sb.Insert(0, (value >> i) & 0x1);
            }

            return sb.ToString();
        }
    }
}
