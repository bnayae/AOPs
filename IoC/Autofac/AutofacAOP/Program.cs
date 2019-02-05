using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// https://nearsoft.com/blog/aspect-oriented-programming-aop-in-net-core-and-c-using-autofac-and-dynamicproxy/

namespace Bnaya.Samples
{
    internal class Program
    {
        private static readonly object _sync = new object();
        private static readonly AsyncLock _lock = new AsyncLock(TimeSpan.FromSeconds(5));

        private static async Task Main(string[] args)
        {
            await RegAsyncTestAsync().ConfigureAwait(false);
            await DecorateTestAsync().ConfigureAwait(false);
            Console.ReadKey();
        }

        private static async Task RegAsyncTestAsync()
        {
            const int RANGE = 1000;
            var builder = new ContainerBuilder();
            var tasks = Enumerable.Range(0, RANGE)
                    .Select(i => Task.Run(async () =>
                    {
                        //lock (_sync)
                        using(await _lock.AcquireAsync())
                        {
                            builder.RegisterType<ConsoleLogger>()
                                            .Keyed<ILogger>(i)
                                            .SingleInstance();
                            builder.Register<IEnumerable<int>>(c =>
                            {
                                var v = c.Resolve<ILogger>();
                                return Enumerable.Range(4, 5);
                            });
                        }
                    }));

            builder.RegisterType<ConsoleLogger>()
                    .As<ILogger>()
                    .SingleInstance();
            await Task.WhenAll(tasks);
            var container = builder.Build();
            for (int i = 0; i < RANGE; i++)
            {
                if (container.IsRegisteredWithKey<ILogger>(i))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("V,");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("X,");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }

        private static async Task DecorateTestAsync()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLogger>()
                    .As<ILogger>()
                    .SingleInstance();

            #region Decorate

            builder.RegisterType<Calculator>()
                    .Decorate<ICalculator>(builder);
            builder.RegisterType<CalculatorX>()
                    .Decorate<ICalculator>(builder) //, decoratorConfig: x => x.InstancePerDependency())
                    .Decorate(builder, "A");
            builder.RegisterType<CalculatorY>()
                    .Decorate<ICalculator>(builder, "A");

            #endregion // Decorate

            var container = builder.Build();

            #region Test

            bool r = container.IsRegistered<ICalculator>();
            bool ra = container.IsRegisteredWithKey<ICalculator>("A");
            ICalculator f = container.Resolve<ICalculator>();
            ICalculator f1 = container.Resolve<ICalculator>();
            if (f != f1)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("NOT SINGLETOE !!!");
                Console.ResetColor();
            }

            Console.WriteLine(f.GetType().Name);
            Console.WriteLine(f);
            int i = await f.AddAsync(1, 2);
            i = f.Sub(17, 20);

            Console.WriteLine();
            ICalculator[] fs = container.Resolve<ICalculator[]>();
            foreach (var item in fs)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"** {item} **");
                Console.ResetColor();
                item.Add(3, 4);
            }

            Console.WriteLine();
            ICalculator fa = container.ResolveKeyed<ICalculator>("A");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("#######  A ######");
            Console.ResetColor();
            fa.Add(9, 8);


            Console.WriteLine();
            ICalculator[] fsa = container.ResolveKeyed<ICalculator[]>("A");
            foreach (var item in fsa)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"##### {item} ######");
                Console.ResetColor();
                item.Add(3, 1);
            }

            #endregion // Test
        }
    }
}
