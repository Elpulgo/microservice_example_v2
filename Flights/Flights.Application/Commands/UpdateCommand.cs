using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Mapper;
using Flights.Application.Notifications;
using Flights.Core;
using Flights.Core.Events;
using MediatR;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Flights.Application.Commands
{
    public class UpdateFlightCommand : IRequest<CommandResponseBase>
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string FlightNumber { get; set; }
        public FlightStatus Status { get; set; }

        public UpdateFlightCommand()
        { }
    }

    public class UpdateFlightHandler
       : BaseFlightCommand, IRequestHandler<UpdateFlightCommand, CommandResponseBase>
    {
        private readonly IMediator m_Mediator;

        public UpdateFlightHandler(
            IFlightEventStorePublisher eventStorePublisher,
            IMediator mediator)
            : base(eventStorePublisher)
            => m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task<CommandResponseBase> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            if (request.Status == FlightStatus.None || !Enum.IsDefined(typeof(FlightStatus), request.Status))
                return new CommandResponseBase()
                {
                    Success = false,
                    Error = $"Can't update flight to status '{request.Status}', invalid status!"
                };

            var eventData = new FlightEventData(request.Map(), EventTypeOperation.Update, "Update flight");

            var response = await base.Handle(eventData, cancellationToken);

            if (response.Success && request.Status.HasFlag(FlightStatus.Arrived))
            {
                await NotifyPassengersOfArrival(request.Id);
            }

            return response;
        }

        private async Task NotifyPassengersOfArrival(Guid flightId)
            => await m_Mediator.Publish(new FlightArrivedNotification(flightId));
    }
}