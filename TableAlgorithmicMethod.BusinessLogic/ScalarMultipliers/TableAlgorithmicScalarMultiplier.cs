using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TableAlgorithmicMethod.BusinessLogic.Arithmetic;

namespace TableAlgorithmicMethod.BusinessLogic.ScalarMultipliers
{
    public class TableAlgorithmicScalarMultiplier : IScalarMultiplier
    {
        public const int MAX_INPUT_VALUES = 30;

        public ScalarMultiplicationResult Multiply(IEnumerable<int> vector1, IEnumerable<int> vector2, IArithmeticOperations arithmeticOperations)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int[] weights = vector1.ToArray();
            int[] inputValues = vector2.ToArray();

            if (vector1.Count() != vector2.Count())
            {
                throw new Exception("Vectors should have same amount of elements");
            }

            ulong numberOfProducts = (ulong)Math.Pow(2, weights.Length);
            uint[] products = new uint[numberOfProducts];

            int maxWeightsExponent = 0;
            int maxInputValuesExponent = 0;
            int[] weightsExponents = new int[weights.Length];
            int[] inputValuesExponents = new int[weights.Length];

            bool skipInputValues = false;
            if (arithmeticOperations is FixedFloatingPointNumbersArithmeticOperations)
            {
                skipInputValues = true;
            }

                for (int i = 0; i < weights.Length; i++)
            {
                int weigthExponent = arithmeticOperations.GetExponent(weights[i]);
                if (weigthExponent > maxWeightsExponent)
                {
                    maxWeightsExponent = weigthExponent;
                }

                weightsExponents[i] = weigthExponent;

                if (skipInputValues)
                {
                    continue;
                }

                int inputValueExponent = arithmeticOperations.GetExponent(inputValues[i]);
                if (inputValueExponent > maxInputValuesExponent)
                {
                    maxInputValuesExponent = inputValueExponent;
                }

                inputValuesExponents[i] = inputValueExponent;
            }

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = arithmeticOperations.MantissaRigthShift(weights[i], maxWeightsExponent - weightsExponents[i]);

                if (skipInputValues)
                {
                    continue;
                }

                inputValues[i] = arithmeticOperations.MantissaRigthShift(inputValues[i], maxInputValuesExponent - inputValuesExponents[i]);
            }

            for (ulong i = 0; i < numberOfProducts; i++)
            {
                uint product = 0;
                for (int j = 0; j < weights.Length; j++)
                {
                    if ((((ulong)1 << j) & i) > 0)
                    {
                        product += (uint)(weights[j] & FloatingPointNumbersArithmeticOperations.MANTISSA_MASK);
                    }
                }

                products[i] = product;
            }

            uint resultExponent = (uint)((maxInputValuesExponent + maxWeightsExponent) << 24);

            uint result = 0;

            ulong[] inputData;
            int count = arithmeticOperations.NumberSize;

            int[] intputValuesMantisses;
            if (arithmeticOperations is FixedFloatingPointNumbersArithmeticOperations fixedFloatingPointNumbersArithmeticOperations)
            {
                intputValuesMantisses = inputValues.Select(d => (int)(fixedFloatingPointNumbersArithmeticOperations.FixedToFloatingPoint(d) & FloatingPointNumbersArithmeticOperations.MANTISSA_MASK)).ToArray();
            }
            else
            {
                intputValuesMantisses = inputValues.Select(d => (int)(d & FloatingPointNumbersArithmeticOperations.MANTISSA_MASK)).ToArray();
            }

            inputData = ConvertToSerialData(intputValuesMantisses, count);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int k = 0; k < 10000; k++)
            {
                result = 0;
                for (int i = 0; i < count; i++)
                {
                    result = products[inputData[i]] + (result >> 1);
                }

                result |= resultExponent;
            }

            sw.Stop();

            if (arithmeticOperations is FloatingPointNumbersArithmeticOperations || arithmeticOperations is FixedFloatingPointNumbersArithmeticOperations)
            {
                int mantissa = (int)((result & 0xFFFFFF) << 7);
                int exponent = (int)(result & 0x7F000000);
                FloatingPointNumbersArithmeticOperations.NormalizeManissa(ref mantissa, ref exponent);

                return new ScalarMultiplicationResult(exponent | mantissa, sw.ElapsedTicks);
            }
            else
            {
                return new ScalarMultiplicationResult((int)result, sw.ElapsedTicks);
            }
        }

        private ulong[] ConvertToSerialData(int[] vector, int numberOfValues)
        {
            if (vector.Length > MAX_INPUT_VALUES)
            {
                throw new Exception($"No more than {MAX_INPUT_VALUES} input values are allowed");
            }

            var result = new ulong[numberOfValues];

            for (int i = 0; i < numberOfValues; i++)
            {
                ulong value = 0;
                for (int j = 0; j < vector.Length; j++)
                {
                    value |= (uint)((vector[j] >> i) & 0x1) << j;
                }

                result[i] = value;
            }

            return result;
        }
    }
}
