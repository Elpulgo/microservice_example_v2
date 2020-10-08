using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Mapper;
using Flights.Application.Responses;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Flights.Application.Commands
{
    public class CreateFlightCommand : IRequest<FlightCommandResponse>
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }

        public CreateFlightCommand()
        { }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Destination))
                return false;
            if (string.IsNullOrEmpty(FlightNumber))
                return false;
            if (string.IsNullOrEmpty(Origin))
                return false;

            return true;
        }
    }

    public class CreateFlightHandler
        : BaseFlightCommand, IRequestHandler<CreateFlightCommand, FlightCommandResponse>
    {
        public CreateFlightHandler(IFlightEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher) { }

        public async Task<FlightCommandResponse> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return new FlightCommandResponse(
                    new CommandResponseBase()
                    {
                        Error = "Destination, origin and flight number can't be empty!",
                        Success = false
                    },
                    Guid.Empty);
            }


            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Create, "Create flight");
            var result = await base.Handle(eventData, cancellationToken);
            return new FlightCommandResponse(result, eventData.Data.Id);
        }
    }
}