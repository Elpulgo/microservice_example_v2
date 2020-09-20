using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;

namespace Flights.Application.Commands
{
    public class DeleteFlightCommand : IRequest<FlightCommandResponse>
    {
        public Guid Id { get; set; }
        public DeleteFlightCommand()
        { }
    }
    public class DeleteFlightHandler
        : BaseFlightCommand, IRequestHandler<DeleteFlightCommand, FlightCommandResponse>
    {
        public DeleteFlightHandler(IFlightEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<FlightCommandResponse> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(
                new Flight() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete flight");

            return await base.Handle(eventData, cancellationToken);
        }
    }
}