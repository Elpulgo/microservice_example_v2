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
        : IRequestHandler<CreatePassengerCommand, PassengerCommandResponse>
    {
        private readonly IPassengerEventStorePublisher m_EventStorePublisher;

        public CreatePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
        {
            m_EventStorePublisher = eventStorePublisher ?? throw new ArgumentNullException(nameof(eventStorePublisher));
        }
        public async Task<PassengerCommandResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Create, "Create passenger");
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