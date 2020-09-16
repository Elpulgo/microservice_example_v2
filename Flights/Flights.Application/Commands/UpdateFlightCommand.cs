using System;
using Flights.Application.Responses;
using Flights.Core;
using MediatR;

namespace Flights.Application.Commands
{
    public class UpdateFlightCommand : IRequest<FlightCommandResponse>
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }
        public FlightStatus Status { get; set; }

        public UpdateFlightCommand()
        {
        }
    }
}