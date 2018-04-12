using System.Threading.Tasks;

namespace SpringIt.ServiceBus
{
    public interface IService
    {
        Task Start();
        void Stop();
    }
}