using System;
using System.Collections.Generic;
using System.Text;

namespace Bnaya.Samples
{
    public class CalculatorDecorator : ICalculator
    {
        private readonly ICalculator _decorated;
        public CalculatorDecorator(ICalculator decorated)
        {
            _decorated = decorated;
        }

        public int Add(int a, int b)
        {
            try
            {
                Console.WriteLine($"Executing {a} + {b}");
                int result = _decorated.Add(a, b);
                Console.WriteLine($"{a} + {b} = {result}");
                return result;
            }
            catch (Exception)
            {
                Console.WriteLine($"Fail to execute {a} + {b}");
                throw;
            }
        }

        public int Sub(int a, int b)
        {
            try
            {
                Console.WriteLine($"Executing {a} - {b}");
                int result = _decorated.Sub(a, b);
                Console.WriteLine($"{a} - {b} = {result}");
                return result;
            }
            catch (Exception)
            {
                Console.WriteLine($"Fail to execute {a} - {b}");
                throw;
            }
        }
    }
}
