using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public static class Extensions
    {
        public static void Decorate<T>(
            this IServiceRegistry container)
        {
            container.Decorate<T>((ioc, instance) =>
            {
                var decorated = DynamicProxyFactory<T>.Create(instance,
                    info => Console.WriteLine($"    BEFORE: {instance} {info.ImplementationName} [{info.ContractName}: {info.MethodName}]"),
                    info => Console.WriteLine($@"   AFTER ({info.Duration:g}): {info.ImplementationName} [{info.ContractName}: {info.MethodName}]
------------------------------"),
                    info => Console.WriteLine($"    ERROR: {info.ImplementationName} [{info.ContractName}: {info.MethodName}]\r\n{info.Error}"));

                return decorated;
            });
        }
    }
}
