using System;
using System.Reflection;
using Common.Logging;
using Topshelf;
using Topshelf.HostConfigurators;

namespace SpringIt.ServiceBus
{
    public static class TopselfExtension
    {
        public static EndpointConfigurator RunTopshelf<TService>(this EndpointConfigurator configurator,
            Action<HostConfigurator> configureCallback, Func<TService> serviceFactory) where TService : class, IService
        {
            HostFactory.Run(c =>
            {
                c.UseNLog();
                LogManager.Reset();

                var a = Assembly.GetEntryAssembly();
                var serviceName = a.GetName().Name;
                var serviceDescription = $"A service hosting {serviceName}";

                c.BeforeInstall(() => Console.WriteLine("[TOPSHELF] Install {0}", serviceName));
                c.BeforeUninstall(() => Console.WriteLine("[TOPSHELF] Uninstall {0}", serviceName));

                c.SetDisplayName(serviceName);
                c.SetServiceName(serviceName);
                c.SetDescription(serviceDescription);

                configureCallback(c);

                c.Service<TService>(s =>
                {
                    s.ConstructUsing(serviceFactory);

                    s.WhenStarted(service =>
                    {
                        var logger = LogManager.GetLogger(service.GetType());
                        logger.InfoFormat("[TOPSHELF]  Starting {0}", serviceName);
                        service.Start().Wait();
                        logger.InfoFormat("[TOPSHELF] Started {0}", serviceName);
                    });

                    s.WhenStopped(service =>
                    {
                        var logger = LogManager.GetLogger(service.GetType());
                        logger.InfoFormat("[TOPSHELF] Stopping {0}", serviceName);
                        service.Stop().Wait();
                        logger.InfoFormat("[TOPSHELF] Stopped {0}", serviceName);
                    });
                });

                c.OnException(exception =>
                {
                    var logger = LogManager.GetLogger("default");
                    logger.FatalFormat("d", exception);
                });
            });


            return configurator;
        }
    }
}