using System;
using System.Threading.Tasks;
using MassTransit;
using SimpleInjector;
using Topshelf.SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    class Program
    {
        private static Container _container;
        static void Main(string[] args)
        {
            _container = CompositionRoot.With.Configure();

            _container.Register<Test>();


            EndpointConfigurator
                .With
                .UseSimpleInjector(_container)
                .ApplyTopshelf(c => c.UseSimpleInjector(_container), _container.GetInstance<IService>);

            
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
