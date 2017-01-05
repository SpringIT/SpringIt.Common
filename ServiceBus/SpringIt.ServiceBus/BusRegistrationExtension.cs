using System;
using MassTransit;
using MassTransit.Policies;
using SpringIt.ServiceBus.Common;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus
{
    public static class BusRegistrationExtension
    {
        public static EndpointConfigurator ApplyBus(this EndpointConfigurator endpointConfigurator, Action<IBusControl> registerBus, Action<IReceiveEndpointConfigurator> configuration, Func<IQueueHelper> queueHelperFactory)
        {
            var bus = BusFactory(configuration, queueHelperFactory);
            registerBus(bus);

            return endpointConfigurator;
        }

        public static EndpointConfigurator ApplyBus(this EndpointConfigurator endpointConfigurator, Action<IBusControl> registerBus, Func<IQueueHelper> queueHelperFactory)
        {
            Action<IReceiveEndpointConfigurator> defaultConfiguration = delegate (IReceiveEndpointConfigurator configurator)
            {
                configurator.UseRetry(new ExponentialRetryPolicy(new AllPolicyExceptionFilter(), 3, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(10)));
            };

            return endpointConfigurator.ApplyBus(registerBus, defaultConfiguration, queueHelperFactory);
        }

        public static IBusControl BusFactory(Action<IReceiveEndpointConfigurator> configuration, Func<IQueueHelper> queueHelperFactory)
        {
        
                var queueHelper = queueHelperFactory();

                var bus = Bus.Factory.CreateUsingRabbitMq(busFactoryConfigurator =>
                {
                    var host = busFactoryConfigurator.Host(queueHelper.Host, h =>
                    {
                        h.Username(queueHelper.Username);
                        h.Password(queueHelper.Password);
                    });

                    busFactoryConfigurator.ReceiveEndpoint(host, queueHelper.Endpoint, configuration.Invoke);
                });

                bus.ConnectReceiveObserver(new ServiceBusObserver());
                bus.ConnectPublishObserver(new ServiceBusObserver());
                bus.ConnectSendObserver(new ServiceBusObserver());

                return bus;
        }

    }
}