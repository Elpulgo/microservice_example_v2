using Passengers.Core;
using Passengers.Core.Models;
using Shared.Infrastructure;

namespace Passengers.Infrastructure
{
    public class PassengerReadRepository : ReadRepository<Passenger>, IPassengerReadRepository
    {
        public PassengerReadRepository(PostgreContext context) : base(context, Passenger.Table)
        {

        }
    }
}