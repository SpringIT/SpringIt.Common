using System;
using Autofac;
using MassTransit;
using SpringIt.ServiceBus.Common.Utils;
using Topshelf.Autofac;

namespace SpringIt.ServiceBus.Autofac
{
    public static class AutofacEndpointConfigurator
    {
        public static EndpointConfigurator UseAutofac(this EndpointConfigurator endpointConfigurator, IContainer container, Func<IFactory, IBusControl> busFactory)
        {
            Func<IService> serviceFactory = container.Resolve<IService>;
            Func<IBusControl> instanceCreator = () => {
                var factory = container.Resolve<IFactory>();
                var bus = busFactory.Invoke(factory);
                return bus;
            };

            var builder = new ContainerBuilder();
            builder.Register(c => instanceCreator()).As<IBus>().As<IBusControl>().SingleInstance();
            builder.Update(container);

            endpointConfigurator.ApplyTopshelf(configurator => configurator.UseAutofacContainer(container), serviceFactory);

            return endpointConfigurator;
        }
    }
}
