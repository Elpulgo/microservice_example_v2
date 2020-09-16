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
    public class CreateFlightHandler : IRequestHandler<CreateFlightCommand, FlightCommandResponse>
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public CreateFlightHandler(IFlightEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher;
        }

        public async Task<FlightCommandResponse> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Create, "Create flight");

            var response = new FlightCommandResponse();

            try
            {
                await m_EventStorePublisher.Publish(eventData);
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
    }
}