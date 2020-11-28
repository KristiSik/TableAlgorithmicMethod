using System.Collections.Generic;
using TableAlgorithmicMethod.Helpers;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.ScalarMultipliers
{
    public interface IScalarMultiplier
    {
        ScalarMultiplicationResult Multiply(IEnumerable<int> vector1, IEnumerable<int> vector2, IArithmeticOperations arithmeticOperations);
    }
}
