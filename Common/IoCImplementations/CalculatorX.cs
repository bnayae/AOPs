using System;
using System.Collections.Generic;
using System.Text;

namespace Bnaya.Samples
{
    public class CalculatorX : ICalculator
    {
        private readonly ILogger _logger;
        public CalculatorX(ILogger logger)
        {
            _logger = logger;
        }

        public int Add(int a, int b)
        {
            _logger.Report("Add is actually running");
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
