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
        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator, Container container)
        {

            Func<IService> serviceFactory = container.GetInstance<IService>;
            Func<IQueueHelper> queueHelperFactory = container.GetInstance<IQueueHelper>;
            Func<IBusControl> instanceCreator = () => BusRegistrationExtension.BusFactory(configurator => { configurator.LoadFrom(container);}, queueHelperFactory);

            var registration = Lifestyle.Singleton.CreateRegistration(instanceCreator, container);
            container.AddRegistration(typeof(IBus), registration);
            container.AddRegistration(typeof(IBusControl), registration);
           
            //endpointConfigurator.ApplyBus(bus => { registration.InitializeInstance(bus);}, queueHelperFactory);
            endpointConfigurator.ApplyTopshelf(configurator => configurator.UseSimpleInjector(container), serviceFactory);

            return endpointConfigurator;
        }

    }
}
