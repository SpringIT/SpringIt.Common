﻿using MassTransit.SimpleInjectorIntegration;
using SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    class Program
    {
        private static Container _container;
        static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();
            _container.Register<MessageConsumer>(Lifestyle.Scoped);

            EndpointConfigurator
                .With
                .UseSimpleInjector(_container, factory =>
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