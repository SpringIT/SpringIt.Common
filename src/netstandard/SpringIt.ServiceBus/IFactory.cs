using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus
{
    public interface IFactory
    {
        IBusControl CreateRabbitMqBus();
        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator);
        IBusControl CreateRabbitMqBus(Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator);

        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator,
            Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator);

        IBusControl CreateInMemoryBus();
        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> busFactoryConfigurator);
        IBusControl CreateInMemoryBus(Action<IInMemoryReceiveEndpointConfigurator> receiveEndpointConfigurator);

        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> busFactoryConfigurator,
            Action<IInMemoryReceiveEndpointConfigurator> receiveEndpointConfigurator);

        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator,IRabbitMqHost, IQueueHelper> busFactoryConfigurator);
    }
}