using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public class Calculator : ICalculator
    {
        private readonly ILogger _logger;
        public Calculator(ILogger logger)
        {
            _logger = logger;
        }

        public int Add(int a, int b)
        {
            _logger.Report("Add is actually running");
            return a + b;
        }

        public async Task<int> AddAsync(int a, int b)
        {
            await Task.Delay(1050).ConfigureAwait(false);
            _logger.Report("Add async is actually running");
            return a + b;
        }

        public int Sub(int a, int b)
        {
            _logger.Report("Sub is actually running");
            return a - b;
        }

        public override string ToString() => this.GetType().Name;
    }
}
