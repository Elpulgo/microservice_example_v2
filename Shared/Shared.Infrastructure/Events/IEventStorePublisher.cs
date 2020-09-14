using System.Threading.Tasks;
using Shared.Core.Models;

namespace Shared.Infrastructure.Events
{
    public interface IEventStorePublisher<T>
    {
        Task Publish(IEventData<T> eventData);
    }
}