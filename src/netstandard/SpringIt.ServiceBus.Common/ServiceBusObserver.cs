using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SpringIt.ServiceBus.Common
{
    public class ServiceBusObserver : IReceiveObserver, ISendObserver, IPublishObserver, IConsumeObserver
    {
        private readonly ILogger _logger;

        public ServiceBusObserver(ILogger logger)
        {
            _logger = logger;
        }


        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PreConsume: {0}", context.Message.GetType()));
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PostConsume: {0}", context.Message.GetType()));
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() =>
                _logger.LogCritical("[{0}]ConsumeFault: Unable to process", exception, context.Message.GetType()));
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PrePublish: {0}", context.Message.GetType()));
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PostPublish: {0}", context.Message.GetType()));
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogCritical("[{0}]PublishFault: Unable to process", exception, context.ContentType));
        }

        public Task PreReceive(ReceiveContext context)
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PreReceive: {0}", context.ContentType));
        }

        public Task PostReceive(ReceiveContext context)
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PostReceive: {0}", context.ContentType));
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("[{0}]Consumed message from {1}", consumerType, context.SourceAddress));
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType,
            Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogCritical("[{0}]ConsumeFault: Unable to process message", exception, consumerType));
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            return Task.Factory.StartNew(() => _logger.LogCritical("[{0}]ReceiveContext: Unable to process", exception, context.ContentType));
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PreSend: {0}", context.Message.GetType()));
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogTrace("PostSend: {0}", context.Message.GetType()));
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _logger.LogCritical("SendFault: Unable to process {0}", exception, context.ContentType));
        }
    }
}