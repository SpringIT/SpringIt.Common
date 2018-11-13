using System;
using System.Threading;
using Autofac;
using SpringIt.Config;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.Autofac.Endpoint
{
    public static class CompositionRoot
    {
        private static readonly Lazy<ContainerBuilder> Container = new Lazy<ContainerBuilder>(() => new ContainerBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);


        public static IContainer Configure(this ContainerBuilder container)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register(context =>
                {
                    var c = context.Resolve<IQueueHelper>();
                    return new Factory(() => c);
                })
                .As<IFactory>()
                .SingleInstance();
            containerBuilder.RegisterType<Service>().As<IService>().SingleInstance();
            containerBuilder.RegisterType<ConfigReader>().As<IConfigReader>();
            containerBuilder.RegisterType<RabbitMqHelper>().As<IQueueHelper>();

            containerBuilder.RegisterType<MessageConsumer>().AsSelf();

            return containerBuilder.Build();
        }

        public static ContainerBuilder With => Container.Value;
    }
}