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

            if (vector1.Count() != vector2.Count())
            {
                throw new Exception("Vectors should have same amount of elements");
            }

            ulong numberOfProducts = (ulong)Math.Pow(2, weights.Length);
            uint[] products = new uint[numberOfProducts];

            for (ulong i = 0; i < numberOfProducts; i++)
            {
                uint product = 0;
                for (int j = 0; j < weights.Length; j++)
                {
                    if ((((ulong)1 << j) & i) > 0)
                    {
                        product += (uint)weights[j];
                    }
                }

                products[i] = product;
            }

            uint result = 0;

            ////Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            ////Thread.CurrentThread.Priority = ThreadPriority.Highest;
            ///

            ulong[] inputData;
            var s = vector2.Select(d => (int)(d & FloatingPointNumbersArithmeticOperations.MANTISSA_MASK)).ToArray();
            int count = arithmeticOperations.NumberSize;
            inputData = ConvertToSerialData(s, count);


            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int k = 0; k < 100000; k++)
            {
                result = 0;
                for (int i = 0; i < count; i++)
                {
                    result = (products[inputData[i]]) + (result >> 1);
                }
            }

            sw.Stop();
            return new ScalarMultiplicationResult((int)result, sw.ElapsedTicks);
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
