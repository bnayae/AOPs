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
                                string name = null,
                                Action<IRegistrationBuilder<TContract, LightweightAdapterActivatorData, DynamicRegistrationStyle>> decoratorConfig = null)
        {
            Func<IComponentContext, TContract, TContract> decoratorFactory = (context, instance) =>
            {
                var decorated = DynamicProxyFactory<TContract>.Create(instance,
                                            info => Console.WriteLine($"\tBEFORE: {info.ImplementationName} [{info.ContractName}: {info.MethodName}]"),
                                            info => Console.WriteLine($"\tAFTER ({info.Duration:g}): {info.ImplementationName} [{info.ContractName}: {info.MethodName}]"),
                                            info => Console.WriteLine($"\tERROR: {info.ImplementationName} [{info.ContractName}: {info.MethodName}]\r\n{info.Error}"));
                return decorated;
            };

            return Decorate<TContract>(builder, ioc, decoratorFactory, name, decoratorConfig);
        }

        public static IRegistrationBuilder<TContract, ConcreteReflectionActivatorData, SingleRegistrationStyle>  
                Decorate<TContract>(
                                this IRegistrationBuilder<TContract, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder, 
                                ContainerBuilder ioc, 
                                Func<IComponentContext, TContract, TContract> decoratorFactory,
                                string name = null,
                                Action<IRegistrationBuilder<TContract, LightweightAdapterActivatorData, DynamicRegistrationStyle>> decoratorConfig = null)
        {
            string key = Guid.NewGuid().ToString();
            builder = builder.Keyed<TContract>(key)
                             .SingleInstance();

            IRegistrationBuilder<TContract, LightweightAdapterActivatorData, DynamicRegistrationStyle> decoration =
                ioc.RegisterDecorator<TContract>(decoratorFactory, key, string.IsNullOrEmpty(name) ? null : name);


            if (decoratorConfig != null)
                decoratorConfig(decoration);
            else
                decoration.SingleInstance();
            return builder;
        }
   }
}
