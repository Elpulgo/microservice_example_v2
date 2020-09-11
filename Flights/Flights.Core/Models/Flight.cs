using Shared.Core;

namespace Flights.Core
{
    public class Flight
    {
        public Flight()
        {

        }

        public TId Id { get; set; }

        public FlightStatus Status { get; set; }
        public string Destination { get; set; }

        public string Origin { get; set; }

        public string FlightNumber { get; set; }

    }
}