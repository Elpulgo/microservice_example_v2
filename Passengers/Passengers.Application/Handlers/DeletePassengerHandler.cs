using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Commands;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;

namespace Passengers.Application.Handlers
{
    public class DeletePassengerHandler : IRequestHandler<DeletePassengerCommand, PassengerCommandResponse>
    {
        private readonly IPassengerEventStorePublisher m_EventStorePublisher;

        public DeletePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }

        public async Task<PassengerCommandResponse> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(
                new Passenger() { Id = request.Id },
                EventTypeOperation.Delete,
                "Delete passenger");

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