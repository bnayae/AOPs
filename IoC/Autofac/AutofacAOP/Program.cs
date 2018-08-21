using Autofac;
using System;

// https://nearsoft.com/blog/aspect-oriented-programming-aop-in-net-core-and-c-using-autofac-and-dynamicproxy/

namespace Bnaya.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLogger>()
                    .As<ILogger>()
                    .SingleInstance();
            KeyedToNon(builder);
            //NonToKeyed(builder);
            //try
            //{
            //    NonToNon_Fail(builder);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.GetBaseException().Message);
            //}
            Console.ReadKey();
        }

        private static void KeyedToNon(ContainerBuilder builder)
        {
            builder.RegisterType<Calculator>()
                    .Keyed<ICalculator>("A")
                    .SingleInstance();
            builder.RegisterDecorator<ICalculator>((inner) => new CalculatorDecorator(inner), "A");
            var container = builder.Build();
            ICalculator f = container.Resolve<ICalculator>();
            int i = f.Add(1, 2);

            Console.WriteLine($"Done {i}");

            Console.ReadKey();
        }

        private static void NonToKeyed(ContainerBuilder builder)
        {
            builder.RegisterType<Calculator>()
                    .As<ICalculator>() 
                    .SingleInstance();
            builder.RegisterDecorator<ICalculator>((inner) => new CalculatorDecorator(inner), null, "A");

            var container = builder.Build();
            ICalculator f = container.ResolveKeyed<ICalculator>("A");
            int i = f.Add(1, 2);

            Console.WriteLine($"Done {i}");
        }

        private static void NonToNon_Fail(ContainerBuilder builder)
        {
            builder.RegisterType<Calculator>()
                    .As<ICalculator>() // Not applicable for this kind of registration
                    .SingleInstance();
            builder.RegisterDecorator<ICalculator>((inner) => new CalculatorDecorator(inner), null);

            var container = builder.Build();
            ICalculator f = container.Resolve<ICalculator>();
            int i = f.Add(1, 2);

            Console.WriteLine($"Done {i}");
        }
    }
}
