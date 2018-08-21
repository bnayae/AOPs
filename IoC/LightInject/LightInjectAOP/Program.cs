using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // 1. Create a new Simple Injector container
            var container = new ServiceContainer();
            //container = container.InterceptWith

            // 2. Configure the container (register)
            container.Register<ICalculator, Calculator>(new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            // Register Decorator (support open generic)
            //container.Decorate<ICalculator, CalculatorDecorator>();
            container.Decorate<ICalculator>();

            ICalculator foo = container.GetInstance<ICalculator>();
            foo.Add(1, 2);
            Console.ReadKey();
        }
    }
}
