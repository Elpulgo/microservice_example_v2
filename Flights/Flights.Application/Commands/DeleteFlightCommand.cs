using System;
using Flights.Application.Responses;
using MediatR;

namespace Flights.Application.Commands
{
    public class DeleteFlightCommand : IRequest<FlightCommandResponse>
    {
        public Guid Id { get; set; }
        public DeleteFlightCommand()
        {
        }
    }
}