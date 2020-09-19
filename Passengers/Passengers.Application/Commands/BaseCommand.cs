using System;
using System.Threading;
using System.Threading.Tasks;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Models;
using Shared.Core.Models;

namespace Passengers.Application.Commands
{
    public class BasePassengerCommand
    {
        private readonly IPassengerEventStorePublisher m_EventStorePublisher;

        public BasePassengerCommand(IPassengerEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }

        public async Task<PassengerCommandResponse> Handle(
            IEventData<Passenger> eventData,
            CancellationToken cancellationToken)
        {
            var response = new PassengerCommandResponse();

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