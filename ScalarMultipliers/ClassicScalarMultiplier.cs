using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TableAlgorithmicMethod.Helpers;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.ScalarMultipliers
{
    public class ClassicScalarMultiplier : IScalarMultiplier
    {
        public ScalarMultiplicationResult Multiply(IEnumerable<int> vector1, IEnumerable<int> vector2, FixedPointNumberFormat fixedPointNumberFormat)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            List<int> a = vector1.ToList();
            List<int> b = vector2.ToList();
            
            if (a.Count != b.Count)
            {
                throw new Exception("Vectors should have same amount of elements");
            }


            // TODO:
            // Show amount of memory occupied on UI
            // Add selector of number of multiplies (k)




            int result = 0;
            int numberOfElements = a.Count;

            ////Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            ////Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int k = 0; k < 100000; k++)
            {
                result = 0;
                for (int i = 0; i < numberOfElements; i++)
                {
                    result = BinaryOperations.Add(result, BinaryOperations.Multiply(a[i], b[i], fixedPointNumberFormat));
                }

            }
            sw.Stop();

            return new ScalarMultiplicationResult(result, sw.ElapsedTicks);
        }
    }
}
