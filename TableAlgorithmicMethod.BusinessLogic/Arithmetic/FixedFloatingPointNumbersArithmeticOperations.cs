using System;

namespace TableAlgorithmicMethod.BusinessLogic.Arithmetic
{
    public class FixedFloatingPointNumbersArithmeticOperations : IArithmeticOperations
    {
        private readonly FixedPointNumberFormat _fixedPointNumberFormat;
        private readonly FixedPointNumbersArithmeticOperations _fixedPointNumbersArithmeticOperations;
        private readonly FloatingPointNumbersArithmeticOperations _floatingPointNumbersArithmeticOperations;

        public FixedFloatingPointNumbersArithmeticOperations(FixedPointNumberFormat fixedPointNumberFormat)
        {
            _fixedPointNumberFormat = fixedPointNumberFormat;
            NumberSize = (int)fixedPointNumberFormat + 1;
            _fixedPointNumbersArithmeticOperations = new FixedPointNumbersArithmeticOperations(fixedPointNumberFormat);
            _floatingPointNumbersArithmeticOperations = new FloatingPointNumbersArithmeticOperations();
        }

        public int NumberSize { get; }


        /// <summary>
        /// Addition operation. Returns floating-point number.
        /// </summary>
        /// <param name="a">Floating-point number.</param>
        /// <param name="b">Floating-point number.</param>
        /// <returns></returns>
        public int Add(int a, int b)
        {
            return _floatingPointNumbersArithmeticOperations.Add(a, b);
        }

        /// <summary>
        /// Get exponent of floating point number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GetExponent(int a)
        {
            return _floatingPointNumbersArithmeticOperations.GetExponent(a);
        }

        /// <summary>
        /// Get mantissa of floating point number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GetMantissa(int a)
        {
            return _floatingPointNumbersArithmeticOperations.GetMantissa(a);
        }

        public int MantissaRigthShift(int a, int shift)
        {
            return _floatingPointNumbersArithmeticOperations.MantissaRigthShift(a, shift);
        }

        public int FixedToFloatingPoint(int a)
        {
            switch (_fixedPointNumberFormat)
            {
                case FixedPointNumberFormat.Q15:
                    return ((a & 0x8000) << 15) | ((a & 0x7FFF) << 9);

                case FixedPointNumberFormat.Q23:
                    return ((a & 0x800000) << 7) | ((a & 0x7FFFFF) << 1);

                case FixedPointNumberFormat.Q31:
                    return ((int)(a & 0x80000000)) | ((a & 0x7FFFFFFF) >> 7);

                default:
                    throw new Exception($"Format {_fixedPointNumberFormat} is not supported");
            }
        }

        /// <summary>
        /// Product operation.
        /// </summary>
        /// <param name="a">Fixed-point number.</param>
        /// <param name="b">Floating-point number.</param>
        /// <returns></returns>
        public int Multiply(int a, int b)
        {
            return _floatingPointNumbersArithmeticOperations.Multiply(a, FixedToFloatingPoint(b));
        }

        public double Error(int a, int b)
        {
            return _floatingPointNumbersArithmeticOperations.Error(a, b);
        }
    }
}
