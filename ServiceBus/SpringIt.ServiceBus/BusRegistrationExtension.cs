using System;
using MassTransit;
using SpringIt.ServiceBus.Common;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus
{
    public static class BusRegistrationExtension
    {

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