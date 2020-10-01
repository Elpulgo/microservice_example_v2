using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Notifications;
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
        private readonly IMediator m_Mediator;

        public DeleteFlightHandler(
            IFlightEventStorePublisher eventStorePublisher,
            IMediator mediator)
            : base(eventStorePublisher)
            => m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task<CommandResponseBase> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(
                new Flight() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete flight");

            var response = await base.Handle(eventData, cancellationToken);

            await m_Mediator.Publish(new FlightDeletedNotification(request.Id));

            return response;
        }
    }
}