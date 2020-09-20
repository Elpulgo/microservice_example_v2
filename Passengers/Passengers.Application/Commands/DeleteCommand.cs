using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;

namespace Passengers.Application.Commands
{
    public class DeletePassengerCommand : IRequest<PassengerCommandResponse>
    {
        public Guid Id { get; set; }
        public DeletePassengerCommand()
        { }
    }

    public class DeletePassengerHandler
        : BasePassengerCommand, IRequestHandler<DeletePassengerCommand, PassengerCommandResponse>
    {
        public DeletePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<PassengerCommandResponse> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(
                new Passenger() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete passenger");

            return await base.Handle(eventData, cancellationToken);
        }
    }
}