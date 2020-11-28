using System;
using System.Text;
using TableAlgorithmicMethod.Helpers;

namespace TableAlgorithmicMethod.Models
{
    public class FixedPointNumber : Number
    {
        private const int VALUE_LENGTH_BITS = 32;

        public FixedPointNumber(string fixedPointBinaryNumber, FixedPointNumberFormat fixedPointNumberFormat)
        {
            if (fixedPointBinaryNumber.Length > VALUE_LENGTH_BITS)
            {
                throw new ArgumentException($"Length of '{nameof(fixedPointBinaryNumber)}' cannot be greater than {VALUE_LENGTH_BITS}");
            }

            ////RawValue = BinaryParser.Parse(fixedPointBinaryNumber, (int)fixedPointNumberFormat);
            Format = fixedPointNumberFormat;
        }

        public FixedPointNumber(int value, FixedPointNumberFormat fixedPointNumberFormat)
        {
            RawValue = value;
            Format = fixedPointNumberFormat;
        }

        public FixedPointNumberFormat Format { get; }

        //public static FixedPointNumber operator *(FixedPointNumber x, FixedPointNumber y)
        //{
        //}

        //public static FixedPointNumber operator +(FixedPointNumber x, FixedPointNumber y)
        //{
        //    ////if (x.Format != y.Format)
        //    ////{
        //    ////    throw new Exception("Data format doesn't match");
        //    ////}

        //    ////int xValue = x.RawValue;
        //    ////int yValue = y.RawValue;

        //    ////if (((xValue >> (int)x.Format) & 0x01) == 1)
        //    ////{
        //    ////    // converting to two's complement
        //    ////    xValue = 
        //    ////}

        //    ////if (((yValue >> (int)y.Format) & 0x01) == 1)
        //    ////{
        //    ////    // converting to two's complement

        //    ////}

        //    ////switch (x.Format)
        //    ////{
        //    ////    case FixedPointNumberFormat.Q15:

        //    ////        return new FixedPointNumber();
        //    ////}
        //var result = x.RawValue + y.RawValue;

        //    return new FixedPointNumber(result, x.Format);
        //}

        public static implicit operator float(FixedPointNumber f)
        {
            float result = 0;
            for (int i = (int)f.Format - 1, j = 1; i >= 0; i--, j++)
            {
                if (((f.RawValue >> i) & 0x1) == 1)
                {
                    result += (float)1/(1 << j);
                }
            } 

            return ((f.RawValue >> (int)f.Format) & 0x1) == 1 && result != 0 ? -1 * result : result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = (int)Format; i >= 0; i--)
            {
                result.Append((RawValue >> i) & 0x1);
            }

            return result.ToString();
        }

        protected override int Add(Number n)
        {
            throw new Exception();
            var result = RawValue + n.RawValue;
        }

        protected override Number Multiply(Number n)
        {
            var fixN = n as FixedPointNumber;
            if (Format != fixN.Format)
            {
                throw new Exception("Data format doesn't match");
            }

            switch (fixN.Format)
            {
                case FixedPointNumberFormat.Q15:
                    return new FixedPointNumber((int)((uint)(((long)(RawValue & 0x7FFF) * (fixN.RawValue & 0x7FFF)) >> 15) | (uint)((RawValue ^ fixN.RawValue) & 0x8000)), Format);

                case FixedPointNumberFormat.Q23:
                    return new FixedPointNumber((int)((uint)(((long)(RawValue & 0x7FFFFF) * (fixN.RawValue & 0x7FFFFF)) >> 23) | (uint)((RawValue ^ fixN.RawValue) & 0x800000)), Format);

                default:
                    throw new Exception($"Format {Format} is not supported");
            }
        }
    }
}
