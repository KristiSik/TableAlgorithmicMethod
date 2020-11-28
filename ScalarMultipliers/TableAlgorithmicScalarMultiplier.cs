using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.ScalarMultipliers
{
    public class TableAlgorithmicScalarMultiplier : IScalarMultiplier
    {
        public ScalarMultiplicationResult Multiply(IEnumerable<int> vector1, IEnumerable<int> vector2, FixedPointNumberFormat fixedPointNumberFormat)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            List<int> weights = vector1.ToList();

            ////if (weights.Count != inputData.Count)
            ////{
            ////    throw new Exception("Vectors should have same amount of elements");
            ////}

            
            ulong numberOfProducts = (ulong)Math.Pow(2, weights.Count);
            uint[] products = new uint[numberOfProducts];

            for (ulong i = 0; i < numberOfProducts; i++)
            {
                uint product = 0;
                for (int j = 0; j < weights.Count; j++)
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


            ulong[] inputData;
            var s = vector2.ToArray();
            int count = (int)fixedPointNumberFormat + 1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int k = 0; k < 100000; k++)
            {
                ////inputData = ConvertToSerialData(s, fixedPointNumberFormat);
                ////int len = inputData.Length;
                result = 0;
                for (int i = 0; i < count; i++)
                {
                    ulong value = 0;
                    for (int j = 0; j < s.Length; j++)
                    {
                        value |= (uint)((s[j] >> i) & 0x1) << j;
                    }

                    result = (products[value]) + (result >> 1);
                }
            }

            sw.Stop();
            return new ScalarMultiplicationResult((int)result, sw.ElapsedTicks);
        }

        private ulong[] ConvertToSerialData(int[] vector, FixedPointNumberFormat fixedPointNumberFormat)
        {
            ////if (vector.Count > 64)
            ////{
            ////    throw new Exception("No more than 64 input values are allowed");
            ////}
            int count = (int)fixedPointNumberFormat + 1;
            var result = new ulong[count];

            for (int i = 0; i < count; i++)
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
