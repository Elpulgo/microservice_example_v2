using System.Collections.Generic;
using System.Linq;
using Flights.Application.Commands;
using Flights.Application.Responses;
using Flights.Core;

namespace Flights.Application.Mapper
{
    public static class FlightMapper
    {
        public static Flight Map(this CreateFlightCommand command)
        {
            return new Flight()
            {
                Destination = command.Destination,
                FlightNumber = command.FlightNumber,
                Origin = command.Origin,
                Status = FlightStatus.WaitingForBoarding
            };
        }

        public static Flight Map(this UpdateFlightCommand command)
        {
            return new Flight()
            {
                Id = command.Id,
                Destination = command.Destination,
                FlightNumber = command.FlightNumber,
                Origin = command.Origin,
                Status = command.Status
            };
        }

        public static IReadOnlyList<FlightResponse> Map(this IReadOnlyList<Flight> flights)
        {
            return flights
                .Select(flight => new FlightResponse()
                {
                    Destination = flight.Destination,
                    FlightNumber = flight.FlightNumber,
                    Id = flight.Id,
                    Origin = flight.Origin,
                    Status = flight.Status
                })
                .ToList();
        }
    }
}