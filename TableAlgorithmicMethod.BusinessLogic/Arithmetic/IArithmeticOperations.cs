namespace TableAlgorithmicMethod.BusinessLogic.Arithmetic
{
    public interface IArithmeticOperations
    {
        /// <summary>
        /// Number of bits, occupied by number presentation format.
        /// </summary>
        int NumberSize { get; }

        /// <summary>
        /// Add two numbers.
        /// </summary>
        int Add(int a, int b);

        /// <summary>
        /// Multiply two numbers.
        /// </summary>
        int Multiply(int a, int b);

        /// <summary>
        /// Get exponent part of a number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        int GetExponent(int a);

        /// <summary>
        /// Get mantissa part of a number.
        /// </summary>
        int GetMantissa(int a);

        /// <summary>
        /// Perform right shifts on mantissa.
        /// </summary>
        int MantissaRigthShift(int a, int shift);

        /// <summary>
        /// Calculate error between two numbers.
        /// </summary>
        double Error(int a, int b);
    }
}
