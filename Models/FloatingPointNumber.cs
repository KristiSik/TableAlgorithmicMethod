using System;
using System.Text;

namespace TableAlgorithmicMethod.Models
{
    public class FloatingPointNumber : Number
    {
        private const int VALUE_LENGTH_BITS = 32;
        private const uint EXPONENT_SIGN_MASK = 0x80000000;
        private const uint EXPONENT_MASK = 0x7F000000;
        private const uint MANTISSA_SIGN_MASK = 0x800000;
        private const uint MANTISSA_MASK = 0x7FFFFF;

        public FloatingPointNumber(string floatingPointBinaryNumber)
        {
            if (floatingPointBinaryNumber.Length > VALUE_LENGTH_BITS)
            {
                throw new ArgumentException($"Length of '{nameof(floatingPointBinaryNumber)}' cannot be greater than {VALUE_LENGTH_BITS}");
            }

            ////RawValue = BinaryParser.Parse(floatingPointBinaryNumber, VALUE_LENGTH_BITS - 1);
        }

        public FloatingPointNumber(int rawValue)
        {
            RawValue = rawValue;
        }

        public static FloatingPointNumber operator *(FloatingPointNumber x, FloatingPointNumber y)
        {
            int mantissa = (sbyte)(((long)(x.RawValue & MANTISSA_MASK) * (y.RawValue & MANTISSA_MASK) >> 16) | (uint)((x.RawValue ^ y.RawValue) & MANTISSA_SIGN_MASK));
            int exponent = (int)(((long)(x.RawValue & EXPONENT_MASK) + (y.RawValue & EXPONENT_MASK) >> 8) | (uint)((x.RawValue ^ y.RawValue) & EXPONENT_SIGN_MASK));
            return new FloatingPointNumber(exponent | mantissa);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = VALUE_LENGTH_BITS - 1; i >= 0; i--)
            {
                result.Append((RawValue >> i) & 0x1);
            }

            return result.ToString();
        }

        protected override int Add(Number n)
        {
            throw new NotImplementedException();
        }

        protected override Number Multiply(Number n)
        {
            throw new NotImplementedException();
        }
    }
}
