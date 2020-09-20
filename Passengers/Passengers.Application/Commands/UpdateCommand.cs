using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Responses;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;

namespace Passengers.Application.Commands
{
    public class UpdatePassengerCommand : IRequest<PassengerCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public string Name { get; set; }
        public PassengerStatus Status { get; set; }

        public UpdatePassengerCommand()
        { }
    }

    public class UpdatePassengerHandler
       : BasePassengerCommand, IRequestHandler<UpdatePassengerCommand, PassengerCommandResponse>
    {
        public UpdatePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<PassengerCommandResponse> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
        {
            if (request.Status == PassengerStatus.None || !Enum.IsDefined(typeof(PassengerStatus), request.Status))
                return new PassengerCommandResponse() { Success = false, Error = $"Can't update status to '{request.Status}', invalid status" };

            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Update, "Update passenger");
            return await base.Handle(eventData, cancellationToken);
        }
    }
}