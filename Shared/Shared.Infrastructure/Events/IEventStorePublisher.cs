using System.Threading.Tasks;

namespace Shared.Infrastructure.Events
{
    public interface IEventStorePublisher<T>
    {
        Task Publish(T @event);
    }
}