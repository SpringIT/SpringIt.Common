using System;
using System.Reflection;
using Common.Logging;
using Topshelf;
using Topshelf.HostConfigurators;

namespace SpringIt.ServiceBus
{
    public static class TopselfExtension
    {
        public static EndpointConfigurator ApplyTopshelf<TService>(this EndpointConfigurator configurator, Action<HostConfigurator> configureCallback, Func<TService> serviceFactory) where TService : class, IService
        {
            HostFactory.Run(c =>
            {
                c.UseNLog();
                LogManager.Reset();

                var a = Assembly.GetEntryAssembly();
                var serviceName = a.GetName().Name;
                var serviceDescription = string.Format("{0} MassTransit service", serviceName);

                c.BeforeInstall(() => Console.WriteLine("Topshelf install {0}", serviceName));
                c.BeforeUninstall(() => Console.WriteLine("Topshelf uninstall {0}", serviceName));

                c.SetDisplayName(serviceName);
                c.SetServiceName(serviceName);
                c.SetDescription(serviceDescription);

                configureCallback(c);

                c.Service<TService>(s =>
                {
                    s.ConstructUsing(serviceFactory);
                   
                    s.WhenStarted(service =>
                    {
                        Console.WriteLine("[TOPSHELF]Starting {0}", serviceName);
                        var logger = LogManager.GetLogger(service.GetType());
                        logger.InfoFormat("Starting {0}", serviceName);
                        service.Start();
                        logger.InfoFormat("Started {0}", serviceName);

                    });

                    s.WhenStopped(service =>
                    {
                        Console.WriteLine("[TOPSHELF]Stopping {0}", serviceName);
                        var logger = LogManager.GetLogger(service.GetType());
                        logger.InfoFormat("Stopping {0}", serviceName);
                        service.Stop();
                        logger.InfoFormat("Stopped {0}", serviceName);
                    });
                });
            });

            return configurator;
        }
    }
}