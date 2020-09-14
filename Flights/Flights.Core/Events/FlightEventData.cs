using Shared.Core.Models;

namespace Flights.Core.Events
{
    public class FlightEventData : EventDataBase, IEventData<Flight>
    {
        public FlightEventData(Flight flight, FlightEventType type, string description) : base()
        {
            Data = flight;
            MetaData = new EventDataMeta(type.ToString(), description);
        }

        public Flight Data { get; }

        public IEventDataMeta MetaData { get; }
    }
}