using System.Threading.Tasks;

namespace Shared.Infrastructure.Events
{
    public interface IEventStoreSubscriber<T>
    {
        Task Subscribe();
    }
}