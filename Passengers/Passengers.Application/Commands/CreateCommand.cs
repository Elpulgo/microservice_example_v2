using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Responses;
using Passengers.Application.RPC;
using Passengers.Core;
using Passengers.Core.Events;
using Shared.Core.Constants;
using Shared.Core.Models;

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
        private readonly IFlightRpcClient m_FlightRpcClient;

        public CreatePassengerHandler(
            IPassengerEventStorePublisher eventStorePublisher,
            IFlightRpcClient flightRpcClient)
            : base(eventStorePublisher)
        {
            m_FlightRpcClient = flightRpcClient;
        }

        public async Task<PassengerCommandResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            var flightExists = await m_FlightRpcClient.FlightExistsAsync(request.FlightId);

            if (!flightExists)
            {
                return new PassengerCommandResponse(new CommandResponseBase()
                {
                    Success = false,
                    Error = $"Flight with id '{request.FlightId}' does not exist, so can't create passenger on that flight!"
                }, Guid.Empty);
            }

            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Create, "Create passenger");
            var result = await base.Handle(eventData, cancellationToken);
            return new PassengerCommandResponse(result, eventData.Data.Id);
        }
    }
}