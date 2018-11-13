using Autofac;
using MassTransit;

namespace SpringIt.ServiceBus.Autofac.Endpoint
{
    internal class Program
    {
        private static IContainer _container;

        private static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();

            EndpointConfigurator
                .With
                .Run(_container, factory =>
                {
                    return factory.CreateInMemoryBus(configurator => { },
                            configurator => { configurator.LoadFrom(_container); })
                        .ConnectAllObservers();
                });
        }
    }
}