using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Moq;
using NUnit.Framework;
using SpringIt.ServiceBus.Common;

namespace SpringIt.ServiceBus.Test
{
    [TestFixture]
    public class SendOnlyBusTest
    {
        private SendOnlyBus _sendOnlyBus;
        private Mock<IBusControl> _bus;
        private Mock<BusHandle> _handle;

        [SetUp]
        public void Setup()
        {
            _handle = new Mock<BusHandle>();
            _bus = new Mock<IBusControl>();

            _sendOnlyBus = new SendOnlyBus(() => _bus.Object);
        }

        [Test]
        public async Task PublishAndDispose()
        {
            _bus.Setup(control => control.Start()).Returns(_handle.Object);
            _bus.Setup(control => control.Publish(It.IsAny<object>(), CancellationToken.None)).Returns(Task.FromResult(0));

            await _sendOnlyBus.Publish(new object { });

            _sendOnlyBus.Dispose();

            _bus.Verify(control => control.Start(),Times.Once);
            _bus.Verify(control => control.Publish(It.IsAny<object>(), CancellationToken.None),Times.Once);
            _handle.Verify(h => h.Dispose(),Times.Once);
        }
    }
}
