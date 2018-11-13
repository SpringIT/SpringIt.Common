using MassTransit;
using SpringIt.ServiceBus.Common;

namespace SpringIt.ServiceBus
{
    public static class DiagnosticExtension
    {
        public static IBusControl ConnectAllObservers(this IBusControl bus)
        {
            return bus.ConnectReceiveObserver()
                .ConnectPublishObserver()
                .ConnectSendObserver()
                .ConnectConsumeObserver();
        }


        public static IBusControl ConnectReceiveObserver(this IBusControl bus)
        {
            bus.ConnectReceiveObserver(new ServiceBusObserver());
            return bus;
        }

        public static IBusControl ConnectPublishObserver(this IBusControl bus)
        {
            bus.ConnectPublishObserver(new ServiceBusObserver());
            return bus;
        }

        public static IBusControl ConnectSendObserver(this IBusControl bus)
        {
            bus.ConnectSendObserver(new ServiceBusObserver());
            return bus;
        }

        public static IBusControl ConnectConsumeObserver(this IBusControl bus)
        {
            bus.ConnectConsumeObserver(new ServiceBusObserver());
            return bus;
        }
    }
}