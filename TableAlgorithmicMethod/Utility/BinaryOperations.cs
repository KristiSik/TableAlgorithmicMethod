using System;
using System.Text;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.Utility
{
    public static class BinaryOperations
    {
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
