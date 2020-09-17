using Passengers.Core.Models;
using Shared.Infrastructure.Events;

namespace Passengers.Core
{
    public interface IPassengerEventStoreSubscriber : IEventStoreSubscriber<Passenger>
    {

    }
}