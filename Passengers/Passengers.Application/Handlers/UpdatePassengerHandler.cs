using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Commands;
using Passengers.Application.Mapper;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;

namespace Passengers.Application.Handlers
{
    public class UpdatePassengerHandler
        : IRequestHandler<UpdatePassengerCommand, PassengerCommandResponse>
    {
        private readonly IPassengerEventStorePublisher m_Publisher;

        public UpdatePassengerHandler(IPassengerEventStorePublisher publisher)
        {
            m_Publisher = publisher;
        }
        public async Task<PassengerCommandResponse> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
        {
            if (request.Status == PassengerStatus.None)
                return new PassengerCommandResponse() { Success = false, Error = "Can't update status to 'None', invalid status" };

            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Update, "Update passenger");

            var response = new PassengerCommandResponse();

            try
            {
                await m_Publisher.Publish(eventData);
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