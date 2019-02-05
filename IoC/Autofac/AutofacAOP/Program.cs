using Autofac;
using System;

// https://nearsoft.com/blog/aspect-oriented-programming-aop-in-net-core-and-c-using-autofac-and-dynamicproxy/

namespace Bnaya.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            DecorateTest();
            Console.ReadKey();
        }

        private static void DecorateTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLogger>()
                    .As<ILogger>()
                    .SingleInstance();

            #region Decorate

            builder.RegisterType<Calculator>()
                    .Decorate<ICalculator>(builder);
            builder.RegisterType<CalculatorX>()
                    .Decorate<ICalculator>(builder)
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
            int i = f.Add(1, 2);

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
