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
        private readonly IPassengerEventStorePublisher m_EventStorePublisher;

        public DeletePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }

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