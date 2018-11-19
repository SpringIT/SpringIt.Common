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

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqReceiveEndpointConfigurator> configureEndpoint)
        {
            return CreateRabbitMqBus(configurator => { }, configureEndpoint);
        }

        public IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator,
            Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator)
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

                configurator.ReceiveEndpoint(host, queueHelper.Endpoint, receiveEndpointConfigurator.Invoke);

                busFactoryConfigurator.Invoke(configurator);
            });
        }

        public IBusControl CreateInMemoryBus()
        {
            return CreateInMemoryBus(configurator => { }, configurator => { });
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus)
        {
            return CreateInMemoryBus(configureBus, configurator => { });
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint)
        {
            return CreateInMemoryBus(configurator => { }, configureEndpoint);
        }

        public IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus,
            Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint)
        {
            return BusFactory.CreateUsingInMemory(configurator =>
            {
                var queueHelper = _queueHelperFactory();

                configurator.ReceiveEndpoint(queueHelper.Endpoint, configureEndpoint.Invoke);

                configureBus.Invoke(configurator);
            });
        }
    }
}