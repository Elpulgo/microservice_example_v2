using Passengers.Application.Commands;
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
                FlightId = command.FlightId
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
    }
}