using Passengers.Core.Models;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Passengers.Core.Events
{
    public class PassengerEventData : EventDataBase, IEventData<Passenger>
    {
        public PassengerEventData(Passenger passenger, EventTypeOperation type, string description) : base()
        {
            Data = passenger;
            MetaData = new EventDataMeta(type, description);
        }

        public Passenger Data { get; }

        public IEventDataMeta MetaData { get; }
    }
}