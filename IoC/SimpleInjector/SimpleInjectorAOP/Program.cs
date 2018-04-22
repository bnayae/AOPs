using SimpleInjector;
using System;

// https://simpleinjector.readthedocs.io/en/latest/aop.html
// https://simpleinjector.readthedocs.io/en/latest/using.html
// https://simpleinjector.org/ReferenceLibrary/
// http://simpleinjector.readthedocs.io/en/latest/advanced.html
// https://simpleinjector.readthedocs.io/en/latest/InterceptionExtensions.html

namespace Bnaya.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Create a new Simple Injector container
            var container = new Container();
            //container = container.InterceptWith

            // 2. Configure the container (register)
            container.Register<ICalculator, Calculator>(Lifestyle.Singleton);

            container.Register<ILogger, ConsoleLogger>(Lifestyle.Singleton);

            // Register Decorator (support open generic)
            container.RegisterDecorator<ICalculator, CalculatorDecorator>(Lifestyle.Singleton);

            // 3. Optionally verify the container's configuration.
            container.Verify();

            // 4. Register the container as MVC3 IDependencyResolver.
            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            container.Verify();
            ICalculator foo = container.GetInstance<ICalculator>();
            foo.Add(1, 2);
            Console.ReadKey();
        }
    }
}
