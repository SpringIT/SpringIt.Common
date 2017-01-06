using MassTransit;

namespace SpringIt.ServiceBus.SimpleInjector.Endpoint
{
    public class Service : IService
    {
        private readonly IBusControl _busControl;
        private BusHandle _handle;

        public Service(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public void Start()
        {
            _handle = _busControl.Start();

            _busControl.Publish(new Message()).Wait();
        }

        public void Stop()
        {
            _handle.Stop();
        }
    }
}