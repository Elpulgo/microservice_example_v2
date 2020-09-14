using Flights.Application.Commands;
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
                Id = command.Id,
                Origin = command.Origin,
                Status = command.Status
            };
        }
    }
}