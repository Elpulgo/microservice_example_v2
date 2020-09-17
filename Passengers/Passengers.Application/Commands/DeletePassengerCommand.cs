using System;
using MediatR;
using Passengers.Application.Responses;

namespace Passengers.Application.Commands
{
    public class DeletePassengerCommand : IRequest<PassengerCommandResponse>
    {
        public Guid Id { get; set; }

        public DeletePassengerCommand()
        {

        }
    }
}