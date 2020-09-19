using Shared.Core.Models;

namespace Flights.Core
{
    public class Flight : Entity
    {
        public static string Table = "Flights";
        public Flight() : base(Table)
        {
            
        }

        public Flight(
            string origin,
            string destination,
            string flightNumber,
            FlightStatus status) : base(Table)
        {
            Origin = origin;
            Destination = destination;
            FlightNumber = flightNumber;
            Status = status;
        }

        public FlightStatus Status { get; set; }
        public string Destination { get; set; }

        public string Origin { get; set; }

        public string FlightNumber { get; set; }
    }
}