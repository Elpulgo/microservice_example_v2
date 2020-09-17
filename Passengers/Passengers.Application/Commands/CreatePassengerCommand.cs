using System;
using MediatR;
using Passengers.Application.Responses;

namespace Passengers.Application.Commands
{
    public class CreatePassengerCommand : IRequest<PassengerCommandResponse>
    {
        public string Name { get; set; }

        public Guid FlightId { get; set; }
        public CreatePassengerCommand()
        {

        }
    }
}