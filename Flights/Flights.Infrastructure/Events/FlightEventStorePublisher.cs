using Flights.Core;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace Flights.Infrastructure
{
    public class FlightEventStorePublisher : EventStorePublisher<Flight>, IFlightEventStorePublisher
    {
        public FlightEventStorePublisher(IEventStoreContext context) : base(context)
        {

        }
    }
}