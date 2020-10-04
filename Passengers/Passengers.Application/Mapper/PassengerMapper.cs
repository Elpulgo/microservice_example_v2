using System.Collections.Generic;
using System.Linq;
using Passengers.Application.Commands;
using Passengers.Application.Responses;
using Passengers.Core.Models;

namespace Passengers.Application.Mapper
{
    public static class PassengerMapper
    {
        public static Passenger Map(this CreatePassengerCommand command)
        {
            return new Passenger()
            {
                Name = command.Name,
                FlightId = command.FlightId,
                Status = PassengerStatus.CheckedIn
            };
        }

        public static Passenger Map(this UpdatePassengerCommand command)
        {
            return new Passenger()
            {
                FlightId = command.FlightId,
                Name = command.Name,
                Status = command.Status,
                Id = command.Id
            };
        }
        public static IReadOnlyList<PassengerResponse> Map(this IReadOnlyList<Passenger> passengers)
        {
            return passengers
                .Select(passenger => new PassengerResponse()
                {
                    FlightId = passenger.FlightId,
                    Id = passenger.Id,
                    Name = passenger.Name,
                    Status = passenger.Status
                })
                .ToList();
        }

        public static PassengerResponse Map(this Passenger passenger)
        {
            return new PassengerResponse()
            {
                FlightId = passenger.FlightId,
                Id = passenger.Id,
                Name = passenger.Name,
                Status = passenger.Status
            };
        }
    }
}