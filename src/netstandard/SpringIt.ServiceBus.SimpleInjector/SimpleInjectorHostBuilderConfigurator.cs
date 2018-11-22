using System;
using System.Collections.Generic;
using SimpleInjector;
using Topshelf.Builders;
using Topshelf.Configurators;
using Topshelf.HostConfigurators;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public class SimpleInjectorHostBuilderConfigurator : HostBuilderConfigurator
    {

        private static Container _container;


        public SimpleInjectorHostBuilderConfigurator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public static Container Container => _container;

        public HostBuilder Configure(HostBuilder builder)
        {
            return builder;
        }

        public IEnumerable<ValidateResult> Validate()
        {
            yield break;
        }
    }
}