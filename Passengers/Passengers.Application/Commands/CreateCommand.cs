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
        : BasePassengerCommand, IRequestHandler<CreatePassengerCommand, PassengerCommandResponse>
    {
        public CreatePassengerHandler(IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<PassengerCommandResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Create, "Create passenger");
            var result = await base.Handle(eventData, cancellationToken);
            return new PassengerCommandResponse(result, eventData.Data.Id);
        }
    }
}