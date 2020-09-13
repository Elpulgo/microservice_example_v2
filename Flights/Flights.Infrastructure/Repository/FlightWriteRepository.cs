using Flights.Core;
using Shared.Infrastructure;

namespace Flights.Infrastructure
{
    public class FlightWriteRepository : WriteRepository<Flight>, IFlightWriteRepository
    {
        public FlightWriteRepository(PostgreContext context) : base(context, Flight.Table)
        {

        }
    }
}