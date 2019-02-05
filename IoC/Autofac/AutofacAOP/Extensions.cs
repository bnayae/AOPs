using Autofac;
using Autofac.Builder;
using Autofac.Features.LightweightAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public static class Extensions
    {
        public static IRegistrationBuilder<TContract, ConcreteReflectionActivatorData, SingleRegistrationStyle>  
                Decorate<TContract>(
                                this IRegistrationBuilder<TContract, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder, 
                                ContainerBuilder ioc, 
                                string name = null)//,
                                //Action<IRegistrationBuilder<TContract, LightweightAdapterActivatorData, DynamicRegistrationStyle>> decoratorConfig = null)
        {
            string key = Guid.NewGuid().ToString();
            builder = builder.Keyed<TContract>(key)
                             .SingleInstance();

            IRegistrationBuilder<TContract, LightweightAdapterActivatorData, DynamicRegistrationStyle> decoration = ioc.RegisterDecorator<TContract>((context, instance) =>
                 {
                     var x = context.ResolveKeyed<TContract>(key);
                     var decorated = DynamicProxyFactory<TContract>.Create(x, //instance,
                                                 info => Console.WriteLine($"\tBEFORE: {info.ImplementationName} [{info.ContractName}: {info.MethodName}]"),
                                                 info => Console.WriteLine($"\tAFTER ({info.Duration:g}): {info.ImplementationName} [{info.ContractName}: {info.MethodName}]"),
                                                 info => Console.WriteLine($"\tERROR: {info.ImplementationName} [{info.ContractName}: {info.MethodName}]\r\n{info.Error}"));
                     return decorated;
                 }, key, string.IsNullOrEmpty(name) ? null : name )
                 .SingleInstance();

            //decoratorConfig?.Invoke(decoration);
            return builder;
        }
   }
}
