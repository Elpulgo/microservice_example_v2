using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Queries;
using Passengers.Core;
using Passengers.Core.Events;
using Passengers.Core.Models;
using Shared.Core.Constants;
using Shared.Core.Models;

namespace Passengers.Application.Commands
{
    public class UpdatePassengerCommand : IRequest<CommandResponseBase>
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public string Name { get; set; }
        public PassengerStatus Status { get; set; }

        public UpdatePassengerCommand()
        { }
    }

    public class UpdatePassengerHandler
       : BasePassengerCommand, IRequestHandler<UpdatePassengerCommand, CommandResponseBase>
    {
        private readonly IMediator m_Mediator;

        public UpdatePassengerHandler(
            IMediator mediator,
            IPassengerEventStorePublisher eventStorePublisher)
            : base(eventStorePublisher)
            => m_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task<CommandResponseBase> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
        {
            if (!ValidatePassengerStatus(request.Status, out CommandResponseBase response))
                return response;

            var (flightExists, existsResponse) = await ValidateFlightExistsAsync(request.FlightId);
            if (!flightExists)
                return existsResponse;

            var eventData = new PassengerEventData(request.Map(), EventTypeOperation.Update, "Update passenger");
            return await base.Handle(eventData, cancellationToken);
        }

        private bool ValidatePassengerStatus(PassengerStatus status, out CommandResponseBase response)
        {
            response = null;

            if (status == PassengerStatus.None || !Enum.IsDefined(typeof(PassengerStatus), status))
            {
                response = new CommandResponseBase()
                {
                    Success = false,
                    Error = $"Can't update status to '{status}', invalid status"
                };

                return false;
            }

            return true;
        }

        private async Task<(bool Exists, CommandResponseBase Response)> ValidateFlightExistsAsync(Guid flightId)
        {
            var flightExists = await m_Mediator.Send(new FlightExistsQuery(flightId));

            if (!flightExists)
            {
                return (
                    Exists: false,
                    Response: new CommandResponseBase()
                    {
                        Success = false,
                        Error = $"Flight with id '{flightId}' does not exist, so can't create passenger on that flight!"
                    });
            }

            return (
                Exists: true,
                Response: null
            );
        }
    }
}