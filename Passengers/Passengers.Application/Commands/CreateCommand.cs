using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Queries;
using Passengers.Application.Responses;
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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            if (FlightId == Guid.Empty)
                return false;
            if (string.IsNullOrEmpty(FlightId.ToString()))
                return false;

            return true;
        }
    }

    public class CreatePassengerHandler
        : BasePassengerCommand, IRequestHandler<CreatePassengerCommand, PassengerCommandResponse>
    {
        private readonly IMediator m_Mediator;

        public CreatePassengerHandler(
            IPassengerEventStorePublisher eventStorePublisher,
            IMediator mediator)
            : base(eventStorePublisher)
            => m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task<PassengerCommandResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return new PassengerCommandResponse(
                    new CommandResponseBase()
                    {
                        Error = "Name and flight id can't be empty!",
                        Success = false
                    },
                    Guid.Empty);
            }

            var (flightExists, response) = await ValidateFlightExistsAsync(request.FlightId);
            if (!flightExists)
                return response;

            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Create, "Create passenger");
            var result = await base.Handle(eventData, cancellationToken);
            return new PassengerCommandResponse(result, eventData.Data.Id);
        }

        private async Task<(bool Exists, PassengerCommandResponse Response)> ValidateFlightExistsAsync(Guid flightId)
        {
            var flightExists = await m_Mediator.Send(new FlightExistsQuery(flightId));

            if (!flightExists)
            {
                return (
                    Exists: false,
                    Response: new PassengerCommandResponse(new CommandResponseBase()
                    {
                        Success = false,
                        Error = $"Flight with id '{flightId}' does not exist, so can't create passenger on that flight!"
                    }, Guid.Empty));
            }

            return (
                Exists: true,
                Response: null
            );
        }
    }
}