using MassTransit;
using Microsoft.Extensions.Logging;
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

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(LogLevel.Trace);
            var logger = loggerFactory.CreateLogger("SpringIt.ServiceBus.SimpleInjector.Endpoint");
            

            EndpointConfigurator
                .With
                .Run(_container, factory =>
                {
                    return factory.CreateInMemoryBus(configurator =>
                        {
                            configurator.UseExtensionsLogging(loggerFactory);
                            //no config steps
                        }, configurator => { configurator.LoadFrom(_container); })
                    .ConnectAllObservers(logger);
                });
        }
    }
}