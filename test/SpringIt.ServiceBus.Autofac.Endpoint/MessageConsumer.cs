using System;
using System.Threading.Tasks;
using MassTransit;

namespace SpringIt.ServiceBus.Autofac.Endpoint
{
    class MessageConsumer: IConsumer<Message>
    {
        public Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine("message");

            return Task.FromResult(0);
        }
    }
}