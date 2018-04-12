using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace SpringIt.ServiceBus
{
    public interface IFactory
    {
        IBusControl CreateRabbitMqBus();
        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> configureBus);
        IBusControl CreateRabbitMqBus(Action<IRabbitMqReceiveEndpointConfigurator> configureEndpoint);
        IBusControl CreateRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator> configureBus, Action<IRabbitMqReceiveEndpointConfigurator> configureEndpoint);
        IBusControl CreateInMemoryBus();
        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus);
        IBusControl CreateInMemoryBus(Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint);
        IBusControl CreateInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configureBus, Action<IInMemoryReceiveEndpointConfigurator> configureEndpoint);
    }
}