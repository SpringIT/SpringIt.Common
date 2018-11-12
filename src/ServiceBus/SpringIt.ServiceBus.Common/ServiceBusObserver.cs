using System;
using System.Threading.Tasks;
using Common.Logging;
using MassTransit;
using MassTransit.Pipeline;

namespace SpringIt.ServiceBus.Common
{
    public class ServiceBusObserver : IReceiveObserver, ISendObserver, IPublishObserver, IConsumeObserver
    {
        private readonly ILog _log = LogManager.GetLogger<ServiceBusObserver>();

        public Task PreReceive(ReceiveContext context)
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PreReceive: {0}", context.ContentType));
        }

        public Task PostReceive(ReceiveContext context)
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PostReceive: {0}", context.ContentType));
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            return Task.Factory.StartNew(() =>_log.TraceFormat("[{0}]Consumed message from {1}", consumerType, context.SourceAddress));
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _log.FatalFormat("[{0}]ConsumeFault: Unable to process message", exception, consumerType));
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            return Task.Factory.StartNew(() => _log.FatalFormat("[{0}]ReceiveContext: Unable to process", exception, context.ContentType));
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PreSend: {0}", context.Message.GetType()));
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PostSend: {0}", context.Message.GetType()));
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _log.FatalFormat("SendFault: Unable to process {0}", exception, context.ContentType));
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PrePublish: {0}", context.Message.GetType()));
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PostPublish: {0}", context.Message.GetType()));
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _log.FatalFormat("[{0}]PublishFault: Unable to process", exception, context.ContentType));
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PreConsume: {0}", context.Message.GetType()));
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.Factory.StartNew(() => _log.TraceFormat("PostConsume: {0}", context.Message.GetType()));
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.Factory.StartNew(() => _log.FatalFormat("[{0}]ConsumeFault: Unable to process", exception, context.Message.GetType()));
        }
    }
}