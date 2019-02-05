using Bnaya.Samples;
using LightInject;
using System;
using System.Collections.Generic;

namespace LightInjectAOP_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LightInject");
            Simple();
            Console.ReadKey();
        }

        private static void Simple()
        {
            // 1. Create a new Simple Injector container
            var container = new ServiceContainer();
            //container = container.InterceptWith

            // 2. Configure the container (register)
            container.Register<ICalculator, Calculator>(new PerContainerLifetime());
            container.Register<ICalculator, Calculator>("A", new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            // Register Decorator (support open generic)
            //container.Decorate<ICalculator, CalculatorDecorator>();
            container.Decorate<ICalculator>();

            ICalculator foo = container.GetInstance<ICalculator>();
            foo.Add(1, 2);
            Console.WriteLine();
            var foos = container.GetInstance<IEnumerable<ICalculator>>();
            foreach (var f in foos)
            {
                f.Add(2, 3);
            }
        }

    }
}
