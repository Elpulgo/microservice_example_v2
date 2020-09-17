using System;
using Shared.Core.Models;

namespace Passengers.Core.Models
{
    public class Passenger : Entity
    {
        public static string Table = "Passengers";

        public Passenger() : base(Table)
        {

        }

        public Passenger(
            Guid flightId,
            string name,
            PassengerStatus status) : base(Table)
        {
            FlightId = flightId;
            Name = name;
            Status = status;
        }

        public Guid FlightId { get; set; }

        public string Name { get; set; }
        public PassengerStatus Status { get; set; }
    }
}