using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Mapper;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;

namespace Flights.Application.Commands
{
    public class CreateFlightCommand : IRequest<FlightCommandResponse>
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }

        public CreateFlightCommand()
        { }
    }

    public class CreateFlightHandler
        : BaseFlightCommand, IRequestHandler<CreateFlightCommand, FlightCommandResponse>
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public CreateFlightHandler(IFlightEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher)
            => m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));

        public async Task<FlightCommandResponse> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Create, "Create flight");
            return await base.Handle(eventData, cancellationToken);
        }
    }
}