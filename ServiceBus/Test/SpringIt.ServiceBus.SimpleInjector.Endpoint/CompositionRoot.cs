using System;
using System.Threading;
using SimpleInjector;
using SpringIt.ConfigReader;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    public static class CompositionRoot
    {
        private static readonly Lazy<Container> Container = new Lazy<Container>(() => new Container(), LazyThreadSafetyMode.ExecutionAndPublication);


        public static Container Configure(this Container container)
        {
            container.RegisterSingleton<IService, Service>();
            container.Register<IConfigReader, ConfigReader.ConfigReader>();
            container.Register<IQueueHelper, RabbitMqHelper>();

            return container;
        }

        public static Container With => Container.Value;
    }
}