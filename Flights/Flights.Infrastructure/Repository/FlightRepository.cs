using Flights.Core;
using Shared.Infrastructure;

namespace Flights.Infrastructure
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        public FlightRepository(PostgreContext context) : base(context)
        {

        }
    }
}