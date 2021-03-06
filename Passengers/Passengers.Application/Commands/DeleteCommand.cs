using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Passengers.Application.Commands
{
    public class DeletePassengerCommand : IRequest<CommandResponseBase>
    {
        public Guid Id { get; set; }
        public DeletePassengerCommand()
        { }
    }

    public class DeletePassengerHandler
        : BasePassengerCommand, IRequestHandler<DeletePassengerCommand, CommandResponseBase>
    {
        public DeletePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<CommandResponseBase> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(
                new Passenger() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete passenger");

            return await base.Handle(eventData, cancellationToken);
        }
    }
}