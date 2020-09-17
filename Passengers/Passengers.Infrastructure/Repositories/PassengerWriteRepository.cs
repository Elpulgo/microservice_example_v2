using Passengers.Core;
using Passengers.Core.Models;
using Shared.Infrastructure;

namespace Passengers.Infrastructure
{
    public class PassengerWriteRepository : WriteRepository<Passenger>, IPassengerWriteRepository
    {
        public PassengerWriteRepository(PostgreContext context) : base(context, Passenger.Table)
        {

        }
    }
}