using System;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using MassTransit.Util;
using NUnit.Framework;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.Test
{
    [TestFixture]
    public class SendOnlyBusInMemoryTest
    {
        private bool _messageProcessed = false;
        private IBusControl _busControl;
        private QH _queueHelper;

        private class Blup
        {
        }

        private class QH : IQueueHelper
        {
            public Uri Host => new Uri($"loopback://localhost/{Endpoint}");

            public string Endpoint => "NEMO";
            public string Username { get; }
            public string Password { get; }
        }
        [SetUp]
        public void Setup()
        {
            _queueHelper = new QH();

            _busControl = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint(_queueHelper.Endpoint, ep =>
                {
                    ep.Handler<Blup>(context =>
                    {
                        _messageProcessed = true;
                        return TaskUtil.Completed;
                    });
                });
            });
         
        }

        [Test]
        public async Task PublishAndDispose()
        {
            var handle = await _busControl.StartAsync();

            var endpoint = await _busControl.GetSendEndpoint(_queueHelper.Host);

            await endpoint.Send(new Blup {});

            await Task.Delay(TimeSpan.FromSeconds(1));

            _messageProcessed.Should().BeTrue();

            await handle.StopAsync();
        }
    }

   
}
