using System;
using MassTransit;
using MassTransit.SimpleInjectorIntegration;
using SimpleInjector;
using SpringIt.ServiceBus.Common.Utils;
using Topshelf.SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public static class SimpleInjectorEndpointConfigurator
    {
        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator, Container container, Action<IBus> configureBus, Action<IReceiveEndpointConfigurator> configureEndpoint)
        {

            Func<IService> serviceFactory = container.GetInstance<IService>;
            Func<IQueueHelper> queueHelperFactory = container.GetInstance<IQueueHelper>;
            Func<IBusControl> instanceCreator =
                () =>
                {
                    var bus = BusRegistrationExtension.BusFactory(configureEndpoint, queueHelperFactory);
                    configureBus(bus);
                    return bus;
                };

            var registration = Lifestyle.Singleton.CreateRegistration(instanceCreator, container);
            container.AddRegistration(typeof(IBus), registration);
            container.AddRegistration(typeof(IBusControl), registration);

            endpointConfigurator.ApplyTopshelf(configurator => configurator.UseSimpleInjector(container), serviceFactory);

            return endpointConfigurator;
        }

        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator, Container container)
        {
            endpointConfigurator.UseSimpleInjector(container, bus => { }, configurator => configurator.LoadFrom(container));
            return endpointConfigurator;
        }
    }
}