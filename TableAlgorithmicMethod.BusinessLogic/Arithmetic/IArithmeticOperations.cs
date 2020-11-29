﻿namespace TableAlgorithmicMethod.BusinessLogic.Arithmetic
{
    public interface IArithmeticOperations
    {
        int NumberSize { get; }

        int Add(int a, int b);

        int Multiply(int a, int b);
    }
}
