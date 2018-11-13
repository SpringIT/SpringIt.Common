using MassTransit;
using SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    internal class Program
    {
        private static Container _container;

        private static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();
            _container.Register<MessageConsumer>();

            EndpointConfigurator
                .With
                .Run(_container, factory =>
                {
                    return factory.CreateInMemoryBus(configurator =>
                        {
                            //no config steps
                        }, configurator => { configurator.LoadFrom(_container); })
                        .ConnectAllObservers();
                });
        }
    }
}