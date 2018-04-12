using System.Threading.Tasks;
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

        public async Task Start()
        {
            _handle = await _busControl.StartAsync();
            await _busControl.Publish(new Message());
        }


        public async Task Stop()
        {
            await _handle.StopAsync();
        }
    }
}