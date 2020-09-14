using Shared.Infrastructure.Events;

namespace Flights.Core
{
    public interface IFlightEventStorePublisher : IEventStorePublisher<Flight>
    {

    }
}