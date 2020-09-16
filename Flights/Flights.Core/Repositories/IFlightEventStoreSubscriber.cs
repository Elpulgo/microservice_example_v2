using Shared.Infrastructure.Events;

namespace Flights.Core
{
    public interface IFlightEventStoreSubscriber : IEventStoreSubscriber<Flight>
    {

    }
}