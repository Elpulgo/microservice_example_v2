using Passengers.Core;
using Passengers.Core.Models;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace Passengers.Infrastructure
{
    public class PassengerEventStorePublisher : EventStorePublisher<Passenger>, IPassengerEventStorePublisher
    {
        public PassengerEventStorePublisher(IEventStoreContext context) : base(context)
        {

        }
    }
}