using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus
{
    public class Factory : IFactory
    {
        private readonly Func<IQueueHelper> _queueHelperFactory;

        public Factory(Func<IQueueHelper> queueHelperFactory)
        {
            _queueHelperFactory = queueHelperFactory;
        }

        private IBusFactorySelector BusFactory => Bus.Factory;

        public IBusControl CreateRabbitMqBus()
        {
            return CreateRabbitMqBus(configurator => { }, configurator => { });
        }

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator)
        {
            return CreateRabbitMqBus(busFactoryConfigurator, configurator => { });
        }

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator)
        {
            return CreateRabbitMqBus(configurator => { }, receiveEndpointConfigurator);
        }

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator,
            Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator)
        {
            if (busFactoryConfigurator == null) throw new ArgumentNullException(nameof(busFactoryConfigurator));
            if (receiveEndpointConfigurator == null) throw new ArgumentNullException(nameof(receiveEndpointConfigurator));

            return CreateRabbitMqBus((configurator, host, queueHelper) =>
            {
                busFactoryConfigurator.Invoke(configurator);
                configurator.ReceiveEndpoint(host, queueHelper.Endpoint, receiveEndpointConfigurator.Invoke);
            });
        }

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator,IRabbitMqHost, IQueueHelper> busFactoryConfigurator)
        {
            if (busFactoryConfigurator == null) throw new ArgumentNullException();

            return BusFactory.CreateUsingRabbitMq(configurator =>
            {
                var queueHelper = _queueHelperFactory();

                var host = configurator.Host(queueHelper.Host, h =>
                {
                    h.Username(queueHelper.Username);
                    h.Password(queueHelper.Password);
                });

                busFactoryConfigurator.Invoke(configurator, host, queueHelper);
            });
        }


        public IBusControl CreateInMemoryBus()
        {
            return CreateInMemoryBus(configurator => { }, configurator => { });
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> busFactoryConfigurator)
        {
            return CreateInMemoryBus(busFactoryConfigurator, configurator => { });
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryReceiveEndpointConfigurator> receiveEndpointConfigurator)
        {
            return CreateInMemoryBus(configurator => { }, receiveEndpointConfigurator);
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> busFactoryConfigurator,
            Action<IInMemoryReceiveEndpointConfigurator> receiveEndpointConfigurator)
        {
            if (busFactoryConfigurator == null) throw new ArgumentNullException(nameof(busFactoryConfigurator));
            if (receiveEndpointConfigurator == null) throw new ArgumentNullException(nameof(receiveEndpointConfigurator));

            return CreateInMemoryBus((configurator, queueHelper) =>
            {
                busFactoryConfigurator.Invoke(configurator);
                configurator.ReceiveEndpoint(queueHelper.Endpoint, receiveEndpointConfigurator.Invoke);
            });
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator, IQueueHelper> busFactoryConfigurator)
        {
            if (busFactoryConfigurator == null) throw new ArgumentNullException();

            return BusFactory.CreateUsingInMemory(configurator =>
            {
                var queueHelper = _queueHelperFactory();

                busFactoryConfigurator.Invoke(configurator, queueHelper);
            });
        }
    }
}