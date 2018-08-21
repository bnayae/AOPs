using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public class CalculatorDecorator : ICalculator, ICalculatorAsync
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

        public async Task<int> AddAsync(int a, int b)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return Add(a, b);
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

        public async Task<int> SubAsync(int a, int b)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return Sub(a, b);
        }
    }
}
