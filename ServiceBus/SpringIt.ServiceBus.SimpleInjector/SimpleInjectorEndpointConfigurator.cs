using MassTransit;
using SimpleInjector;
using SpringIt.ServiceBus.Common.Utils;
using Topshelf.SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public static class SimpleInjectorEndpointConfigurator
    {
        public static EndpointConfigurator UseSimpleInjector<TService>(this EndpointConfigurator endpointConfigurator, Container container) where TService : class, IService
        {
            return endpointConfigurator.ApplyTopshelf(configurator => configurator.UseSimpleInjector(container),
                container.GetInstance<TService>).ApplyBus(
                bus =>
                {
                    container.Register<IBus>(() => bus);
                    container.Register<IBusControl>(() => bus);
                }, container.GetInstance<IQueueHelper>);
        }
    }
}
