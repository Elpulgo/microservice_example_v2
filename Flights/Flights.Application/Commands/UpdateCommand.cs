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
    public class UpdateFlightCommand : IRequest<FlightCommandResponse>
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }
        public FlightStatus Status { get; set; }

        public UpdateFlightCommand()
        { }
    }

    public class UpdateFlightHandler
       : BaseFlightCommand, IRequestHandler<UpdateFlightCommand, FlightCommandResponse>
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public UpdateFlightHandler(IFlightEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher)
            => m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));

        public async Task<FlightCommandResponse> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            if (request.Status == FlightStatus.None)
                return new FlightCommandResponse() { Success = false, Error = "Can't update flight to status 'None', invalid status!" };

            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Update, "Update flight");

            var response = await base.Handle(eventData, cancellationToken);

            if (response.Success && request.Status.HasFlag(FlightStatus.Arrived))
            {
                await NotifyPassengersOfArrival(request);
            }

            return response;
        }

        private async Task NotifyPassengersOfArrival(UpdateFlightCommand request)
        {
            throw new NotImplementedException();
        }
    }
}