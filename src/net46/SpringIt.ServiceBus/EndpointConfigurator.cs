using System;
using System.Threading;

namespace SpringIt.ServiceBus
{
    public class EndpointConfigurator
    {
        private static readonly Lazy<EndpointConfigurator> LazyConfigurator = new Lazy<EndpointConfigurator>(() => new EndpointConfigurator(), LazyThreadSafetyMode.ExecutionAndPublication);
        public static readonly EndpointConfigurator With = LazyConfigurator.Value;
    }
}