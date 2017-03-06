﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using MassTransit.Util;
using Moq;
using NUnit.Framework;
using SpringIt.ServiceBus.Common;
using SpringIt.ServiceBus.Common.Utils;

namespace SpringIt.ServiceBus.Test
{
    [TestFixture]
    public class SendOnlyBusInMemoryTest
    {
        private SendOnlyBus _sendOnlyBus;
        private bool _messageProcessed = false;

        private class Blup
        {
        }

        private class QH : IQueueHelper
        {
            public Uri Host { get; }
            public string Endpoint => "NEMO";
            public string Username { get; }
            public string Password { get; }
        }
        [SetUp]
        public void Setup()
        {

            _sendOnlyBus = new SendOnlyBus(() =>
            {
                Func<IQueueHelper> qh =()=> new QH();

                var factory = new Factory(qh);

                var bus = factory.CreateInMemoryBus(configurator => { }, configurator =>
                {
                    configurator.Handler<Blup>(context =>
                    {
                        _messageProcessed = true;
                        return TaskUtil.Completed;
                    });
                });

                return bus;
            });
        }

        [Test]
        public async Task PublishAndDispose()
        {

            await _sendOnlyBus.Publish(new Blup {});

            await Task.Delay(TimeSpan.FromSeconds(1));

            _messageProcessed.Should().BeTrue();

        }
    }

   
}
