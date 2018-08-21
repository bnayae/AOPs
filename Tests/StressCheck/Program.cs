using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    class Program
    {
        private const int LIMIT = 1_000_000;


        static void Main(string[] args)
        {
            var calc = new Calculator(new Log());
            //Decorated(calc);
            NotDecorated(calc);
        }

        private static void Decorated(ICalculator calc)
        {
            ICalculator decorated = DynamicProxyFactory<ICalculator>.Create(calc);
            var sw = Stopwatch.StartNew();
            int i = 0;
            var q = new Queue<int>();
            while (true)
            {
                int r = decorated.Add(1, i % 100);
                q.Enqueue(r);
                if (++i % LIMIT == 0)
                {
                    sw.Stop();
                    Console.WriteLine($"Decorated Duration = {sw.Elapsed:g} for {LIMIT:N0}");
                    i = 0;
                    if (q.First() < 0)
                        throw new Exception("won't happen");
                    sw.Restart();
                    q.Clear();
                }
            }
        }

        private static void NotDecorated(ICalculator calc)
        {
            var sw = Stopwatch.StartNew();
            int i = 0;
            var q = new Queue<int>();
            while (true)
            {
                int r = calc.Add(1, i % 100);
                q.Enqueue(r);
                if (++i % LIMIT == 0)
                {
                    sw.Stop();
                    Console.WriteLine($"Decorated Duration = {sw.Elapsed:g} for {LIMIT:N0}");
                    i = 0;
                    if (q.First() < 0)
                        throw new Exception("won't happen");
                    sw.Restart();
                    q.Clear();
                }
            }
        }

        private class Log : ILogger
        {
            public void Report(string data)
            {
            }
        }
    }
}
