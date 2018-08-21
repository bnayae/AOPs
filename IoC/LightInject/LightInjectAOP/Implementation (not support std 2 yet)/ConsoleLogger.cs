using System;
using System.Collections.Generic;
using System.Text;

namespace Bnaya.Samples
{
    public class ConsoleLogger : ILogger
    {
        public void Report(string data)
        {
            Console.WriteLine($"Log: {data}");
        }
    }
}
