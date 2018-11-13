using System;
using MassTransit;
using SimpleInjector;
using Topshelf.SimpleInjector;

namespace SpringIt.ServiceBus
{
    public static class SimpleInjectorEndpointConfigurator
    {
        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator, Container container, Func<IFactory, IBusControl> busFactory)
        {

            Func<IBusControl> instanceCreator = () => {
                    var factory = container.GetInstance<IFactory>();
                    var bus = busFactory.Invoke(factory);
                    return bus;
                };

            var registration = Lifestyle.Singleton.CreateRegistration(instanceCreator, container);
            container.AddRegistration(typeof(IBus), registration);
            container.AddRegistration(typeof(IBusControl), registration);


            return endpointConfigurator;
        }

        public static EndpointConfigurator Run(this EndpointConfigurator endpointConfigurator, Container container, Func<IFactory, IBusControl> busFactory)
        {
            Func<IService> serviceFactory = container.GetInstance<IService>;

            return endpointConfigurator
                .UseSimpleInjector(container, busFactory)
                .RunTopshelf(configurator => configurator.UseSimpleInjector(container), serviceFactory);

        }


    }
}