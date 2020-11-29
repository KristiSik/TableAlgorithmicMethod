using System.Collections.Generic;
using TableAlgorithmicMethod.BusinessLogic.Arithmetic;

namespace TableAlgorithmicMethod.BusinessLogic.ScalarMultipliers
{
    public interface IScalarMultiplier
    {
        ScalarMultiplicationResult Multiply(IEnumerable<int> vector1, IEnumerable<int> vector2, IArithmeticOperations arithmeticOperations);
    }
}
