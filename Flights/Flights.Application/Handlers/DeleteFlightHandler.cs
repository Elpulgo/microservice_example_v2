using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;

namespace Flights.Application.Handlers
{
    public class DeleteFlightHandler : IRequestHandler<DeleteFlightCommand, FlightCommandResponse>
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public DeleteFlightHandler(IFlightEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }

        public async Task<FlightCommandResponse> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var eventData = new FlightEventData(
                new Flight() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete flight");

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