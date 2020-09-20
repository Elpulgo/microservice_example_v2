using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Flights.Application.Commands
{
    public class DeleteFlightCommand : IRequest<CommandResponseBase>
    {
        public Guid Id { get; set; }
        public DeleteFlightCommand()
        { }
    }
    public class DeleteFlightHandler
        : BaseFlightCommand, IRequestHandler<DeleteFlightCommand, CommandResponseBase>
    {
        public DeleteFlightHandler(IFlightEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<CommandResponseBase> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(
                new Flight() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete flight");

            return await base.Handle(eventData, cancellationToken);
        }
    }
}