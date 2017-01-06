using System;
using Autofac;
using MassTransit;
using SpringIt.ServiceBus.Common.Utils;
using Topshelf.Autofac;

namespace SpringIt.ServiceBus.Autofac
{
    public static class AutofacEndpointConfigurator
    {
        public static EndpointConfigurator UseAutofac(this EndpointConfigurator endpointConfigurator, IContainer container)
        {
            Func<IService> serviceFactory = container.Resolve<IService>;
            Func<IQueueHelper> queueHelperFactory = container.Resolve<IQueueHelper>;
            Func<IBusControl> instanceCreator = () => BusRegistrationExtension.BusFactory(configurator => { configurator.LoadFrom(container); }, queueHelperFactory);

            var updateBuilder = new ContainerBuilder();
            updateBuilder.Register(c => instanceCreator()).As<IBus>().As<IBusControl>().SingleInstance();
            updateBuilder.Update(container);

            endpointConfigurator.ApplyTopshelf(configurator => configurator.UseAutofacContainer(container), serviceFactory);

            return endpointConfigurator;
        }
    }
}
