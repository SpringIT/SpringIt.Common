using System;

namespace SpringIt.ServiceBus.Common.Utils
{
    public interface IQueueHelper
    {
        Uri Host { get; }
        string Endpoint { get; }

        string Username { get; }
        string Password { get; }
    }
}