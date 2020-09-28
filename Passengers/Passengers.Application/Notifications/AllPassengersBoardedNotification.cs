using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.RPC;

namespace Passengers.Application.Notifications
{
    public class AllPassengersBoardedNotification : INotification
    {
        public Guid FlightId { get; set; }
        public AllPassengersBoardedNotification(Guid flightId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            FlightId = flightId;
        }
    }

    public class UpdateFlightStatusHandler
    : INotificationHandler<AllPassengersBoardedNotification>
    {
        private readonly IFlightRpcClient m_FlightRpcClient;

        public UpdateFlightStatusHandler(IFlightRpcClient flightRpcClient)
            => m_FlightRpcClient = flightRpcClient ?? throw new ArgumentNullException(nameof(flightRpcClient));

        public async Task Handle(AllPassengersBoardedNotification notification, CancellationToken cancellationToken)
            => await m_FlightRpcClient.AllPassengersBoardedAsync(notification.FlightId);
    }
}