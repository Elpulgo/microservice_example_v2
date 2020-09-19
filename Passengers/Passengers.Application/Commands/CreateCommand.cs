using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Shared.Core.Constants;

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

    public class CreatePassengerHandler
        : BasePassengerCommand, IRequestHandler<CreatePassengerCommand, PassengerCommandResponse>
    {
        private readonly IPassengerEventStorePublisher m_EventStorePublisher;

        public CreatePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }
        public async Task<PassengerCommandResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Create, "Create passenger");
            return await base.Handle(eventData, cancellationToken);
        }
    }
}