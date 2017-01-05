using System;
using System.Threading.Tasks;
using Common.Logging;
using MassTransit;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.Common
{
    public class SendOnlyBus : ISendOnlyBus, IDisposable
    {
        private readonly IBusControl _bus;
        private readonly BusHandle _handle;
        private bool _disposed = false;

        private readonly ILog _log = LogManager.GetLogger<SendOnlyBus>();

        public SendOnlyBus(IQueueHelper queueHelper)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(c =>
            {
                c.Host(queueHelper.Host, h =>
                {
                    h.Username(queueHelper.Username);
                    h.Password(queueHelper.Password);
                });
            });

            _bus.ConnectPublishObserver(new ServiceBusObserver());

            _handle = _bus.Start();

            _log.DebugFormat("Bus Address: {0}", _bus.Address);
        }

        public Task Publish<T>(T message) where T : class
        {
            return _bus.Publish(message);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose(); //calling dispose of the handle will internally call stop;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
