using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Responses;
using Flights.Core;
using Shared.Core.Models;

namespace Flights.Application.Commands
{
    public class BaseFlightCommand
    {
        private readonly IFlightEventStorePublisher m_EventStorePublisher;

        public BaseFlightCommand(IFlightEventStorePublisher eventStorePublisher)
            => m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));

        public async Task<FlightCommandResponse> Handle(
            IEventData<Flight> eventData,
            CancellationToken cancellationToken)
        {
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