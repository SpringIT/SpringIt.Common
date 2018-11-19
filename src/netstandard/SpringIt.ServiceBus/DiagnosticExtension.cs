using MassTransit;
using Microsoft.Extensions.Logging;
using SpringIt.ServiceBus.Common;


namespace SpringIt.ServiceBus
{
    public static class DiagnosticExtension
    {
        public static IBusControl ConnectAllObservers(this IBusControl bus, ILogger logger)
        {
            var observer = new ServiceBusObserver(logger);

            return bus.ReceiveObserver(observer)
                .PublishObserver(observer)
                .SendObserver(observer)
                .ConsumeObserver(observer);
        }


        public static IBusControl ReceiveObserver(this IBusControl bus, ServiceBusObserver observer)
        {
            bus.ConnectReceiveObserver(observer);
            return bus;
        }

        public static IBusControl PublishObserver(this IBusControl bus, ServiceBusObserver observer)
        {
            bus.ConnectPublishObserver(observer);
            return bus;
        }

        public static IBusControl SendObserver(this IBusControl bus, ServiceBusObserver observer)
        {
            bus.ConnectSendObserver(observer);
            return bus;
        }

        public static IBusControl ConsumeObserver(this IBusControl bus, ServiceBusObserver observer)
        {
            bus.ConnectConsumeObserver(observer);
            return bus;
        }
    }
}