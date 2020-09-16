using Shared.Core.Constants;
using Shared.Core.Models;

namespace Flights.Core.Events
{
    public class FlightEventData : EventDataBase, IEventData<Flight>
    {
        public FlightEventData(Flight flight, EventTypeOperation type, string description) : base()
        {
            Data = flight;
            MetaData = new EventDataMeta(type, description);
        }

        public Flight Data { get; }

        public IEventDataMeta MetaData { get; }
    }
}