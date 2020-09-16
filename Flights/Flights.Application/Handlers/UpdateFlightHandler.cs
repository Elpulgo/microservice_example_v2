using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Application.Mapper;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;

namespace Flights.Application.Handlers
{
    public class UpdateFlightHandler : IRequestHandler<UpdateFlightCommand, FlightCommandResponse>
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public UpdateFlightHandler(IFlightEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher;
        }

        public async Task<FlightCommandResponse> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            if (request.Status == FlightStatus.None)
                return new FlightCommandResponse() { Success = false, Error = "Can't update flight to status 'None', invalid status!" };

            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Update, "Update flight");

            var response = new FlightCommandResponse();

            try
            {
                await m_EventStorePublisher.Publish(eventData);

                if (request.Status.HasFlag(FlightStatus.Arrived))
                {
                    await NotifyPassengersOfArrival(request);
                }

                response.Success = true;
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Error = exception.Message;
                response.StackTrace = exception.StackTrace;
            }

            return response;
        }

        private async Task NotifyPassengersOfArrival(UpdateFlightCommand request)
        {
            throw new NotImplementedException();
        }
    }
}