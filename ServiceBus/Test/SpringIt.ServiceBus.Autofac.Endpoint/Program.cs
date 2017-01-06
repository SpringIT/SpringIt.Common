using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;

namespace SpringIt.ServiceBus.Autofac.Endpoint
{
    class Program
    {
        private static IContainer _container;
        static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();

            EndpointConfigurator
                .With
                .UseAutofac(_container);

        }
    }

    class Test: IConsumer<Message>
    {
        public Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine("message");

            return Task.FromResult(0);
        }
    }

    public class Message
    {
    }
}
