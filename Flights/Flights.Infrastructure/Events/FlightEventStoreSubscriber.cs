using Flights.Core;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace Flights.Infrastructure
{
    public class FlightEventStoreSubscriber : EventStoreSubscriber<Flight>, IFlightEventStoreSubscriber
    {
        public FlightEventStoreSubscriber(
            IProcessedEventCountHandler processedEventCountHandler,
            IEventStoreContext context,
            IFlightWriteRepository writeRepository,
            string groupName)
            : base(processedEventCountHandler, context, writeRepository, groupName)
        { }
    }
}