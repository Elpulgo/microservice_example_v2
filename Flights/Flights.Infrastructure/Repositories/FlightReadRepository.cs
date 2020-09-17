using Flights.Core;
using Shared.Infrastructure;

namespace Flights.Infrastructure
{
    public class FlightReadRepository : ReadRepository<Flight>, IFlightReadRepository
    {
        public FlightReadRepository(PostgreContext context) : base(context, Flight.Table)
        {

        }
    }
}