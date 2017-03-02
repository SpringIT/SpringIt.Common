using System.Threading.Tasks;

namespace SpringIt.ServiceBus.Common
{
    public interface ISendOnlyBus
    {
        Task Publish<T>(T message) where T : class;
    }
}