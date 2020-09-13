using System;
using Flights.Core;

namespace Flights.Application.Responses
{
    public class FlightResponse
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }
        public FlightStatus Status { get; set; }
    }
}