using System;
using MassTransit;
using SimpleInjector;
using Topshelf.SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public static class SimpleInjectorEndpointConfigurator
    {
        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator, Container container, Func<IFactory, IBusControl> busFactory)
        {

            Func<IService> serviceFactory = container.GetInstance<IService>;
            Func<IBusControl> instanceCreator = () => {
                    var factory = container.GetInstance<IFactory>();
                    var bus = busFactory.Invoke(factory);
                    return bus;
                };

            var registration = Lifestyle.Singleton.CreateRegistration(instanceCreator, container);
            container.AddRegistration(typeof(IBus), registration);
            container.AddRegistration(typeof(IBusControl), registration);

            endpointConfigurator.ApplyTopshelf(configurator => configurator.UseSimpleInjector(container), serviceFactory);

            return endpointConfigurator;
        }
    }
}