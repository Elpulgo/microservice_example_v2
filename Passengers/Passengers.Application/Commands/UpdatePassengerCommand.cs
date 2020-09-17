using System;
using MediatR;
using Passengers.Application.Responses;
using Passengers.Core.Models;

namespace Passengers.Application.Commands
{
    public class UpdatePassengerCommand : IRequest<PassengerCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public string Name { get; set; }
        public PassengerStatus Status { get; set; }

        public UpdatePassengerCommand()
        {

        }
    }
}