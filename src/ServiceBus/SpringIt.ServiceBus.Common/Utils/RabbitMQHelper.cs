using System;
using SpringIt.ConfigReader;

namespace SpringIt.ServiceBus.Common.Utils
{
    public class RabbitMqHelper : IQueueHelper
    {
        private readonly IConfigReader _settings;

        private readonly Lazy<string> _host;
        private readonly Lazy<string> _endpoint;
        private readonly Lazy<string> _username;
        private readonly Lazy<string> _password;


        private const string UsernameKey = "masstransit.rabbitmq.username";
        private const string PasswordKey = "masstransit.rabbitmq.password";
        private const string HostKey = "masstransit.rabbitmq.host";
        private const string EndpointKey = "masstransit.rabbitmq.endpoint";


        public RabbitMqHelper(IConfigReader settings)
        {
            _settings = settings;

            _host = new Lazy<string>(() => _settings.GetValue<string>(HostKey));
            _endpoint = new Lazy<string>(() => _settings.GetValue<string>(EndpointKey));
            _username = new Lazy<string>(() => _settings.GetValue<string>(UsernameKey));
            _password = new Lazy<string>(() => _settings.GetValue<string>(PasswordKey));
        }

        public Uri Host
        {
            get
            {
                var ub = new UriBuilder
                {
                    Scheme = "rabbitmq://",
                    Host = _host.Value
                };
                return ub.Uri;
            }
        }

        public string Endpoint
        {
            get { return _endpoint.Value; }
        }

        public string Username
        {
            get { return _username.Value; }
        }

        public string Password { get { return _password.Value; } }
    }
}