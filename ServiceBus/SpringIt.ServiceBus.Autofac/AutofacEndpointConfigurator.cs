using Autofac;
using Autofac.Core;
using MassTransit;
using SpringIt.ServiceBus.Common.Utils;
using Topshelf.Autofac;

namespace SpringIt.ServiceBus.Autofac
{
    public static class AutofacEndpointConfigurator
    {
        public static EndpointConfigurator UseAutofac<TService>(this EndpointConfigurator endpointConfigurator, Container container) where TService : class, IService
        {
            return endpointConfigurator.ApplyTopshelf(configurator => configurator.UseAutofacContainer(container),
                container.Resolve<TService>).ApplyBus(
                bus =>
                {
                    var updateBuilder = new ContainerBuilder();
                    updateBuilder.RegisterInstance(bus).As<IBus>().As<IBusControl>().SingleInstance();
                    updateBuilder.Update(container);
                }, container.Resolve<IQueueHelper>);
        }
    }
}
