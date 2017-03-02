using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using MassTransit;
using MassTransit.Pipeline;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.Common
{
    public class SendOnlyBus : ISendOnlyBus, IDisposable
    {
        private readonly Lazy<IBusControl> _lazyBus;
        private BusHandle _handle;
        private bool _disposed = false;

        private readonly ILog _log = LogManager.GetLogger<SendOnlyBus>();
        private static readonly object HandleLock = new object();

        public SendOnlyBus(Func<IBusControl> busFactory)
        {
            _lazyBus = new Lazy<IBusControl>(busFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public async Task Publish<T>(T message) where T : class
        {
            lock (HandleLock)
            {
                if (_handle == null)
                {
                    _handle = _lazyBus.Value.Start();
                }
            }

            await _lazyBus.Value.Publish(message).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                lock (_handle)
                {
                    _handle?.Dispose(); //calling dispose of the handle will internally call stop;
                }
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
