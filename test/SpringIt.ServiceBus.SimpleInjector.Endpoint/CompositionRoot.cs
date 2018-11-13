using System;
using System.Threading;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SpringIt.Config;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    public static class CompositionRoot
    {
        private static readonly Lazy<Container> Container = new Lazy<Container>(() => new Container(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static Container Configure(this Container container)
        {
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            container.RegisterSingleton<IFactory>(() => new Factory(container.GetInstance<IQueueHelper>));
            container.RegisterSingleton<IService, Service>();
            container.Register<IConfigReader, ConfigReader>();
            container.Register<IQueueHelper, RabbitMqHelper>();

            return container;
        }

        public static Container With => Container.Value;
    }
}