using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace SpringIt.ServiceBus
{
    public interface IFactory
    {
        IBusControl CreateRabbitMqBus();
        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator);
        IBusControl CreateRabbitMqBus(Action<IRabbitMqReceiveEndpointConfigurator> configureEndpoint);

        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> busFactoryConfigurator,
            Action<IRabbitMqReceiveEndpointConfigurator> receiveEndpointConfigurator);

        IBusControl CreateInMemoryBus();
        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus);
        IBusControl CreateInMemoryBus(Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint);

        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus,
            Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint);
    }
}