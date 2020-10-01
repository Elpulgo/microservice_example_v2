using Passengers.Core;
using Passengers.Core.Models;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace Passengers.Infrastructure
{
    public class PassengerEventStoreSubscriber : EventStoreSubscriber<Passenger>, IPassengerEventStoreSubscriber
    {
        public PassengerEventStoreSubscriber(
            IEventStoreContext context,
            IPassengerWriteRepository writeRepository,
            string groupName)
            : base(context, writeRepository, groupName)
        { }
    }
}