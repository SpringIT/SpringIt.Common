using Autofac;
using MassTransit;

namespace SpringIt.ServiceBus.Autofac.Endpoint
{
    class Program
    {
        private static IContainer _container;
        static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();

            EndpointConfigurator
                .With
                .UseAutofac(_container, factory =>
                {
                    return factory.CreateInMemoryBus(configurator =>
                        {

                        }, configurator =>
                        {
                            configurator.LoadFrom(_container);
                        })
                        .ConnectAllObservers();
                });
        }
    }
}
