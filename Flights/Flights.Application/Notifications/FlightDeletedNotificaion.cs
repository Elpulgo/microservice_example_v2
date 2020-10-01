using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.RPC;
using MediatR;

namespace Flights.Application.Notifications
{

    public class FlightDeletedNotification : INotification
    {
        public Guid FlightId { get; set; }

        public FlightDeletedNotification(Guid flightId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            FlightId = flightId;
        }
    }

    public class NotifyPassengersFlightDeletedHandler
    : INotificationHandler<FlightDeletedNotification>
    {
        private readonly IPassengerRpcClient m_PassengerRpcClient;

        public NotifyPassengersFlightDeletedHandler(IPassengerRpcClient passengerRpcClient)
            => m_PassengerRpcClient = passengerRpcClient ?? throw new ArgumentNullException(nameof(passengerRpcClient));

        public async Task Handle(FlightDeletedNotification notification, CancellationToken cancellationToken)
        {
            var result = await m_PassengerRpcClient.FlightDeletedAsync(notification.FlightId);
            if (result.Success)
                return;

            Console.WriteLine($"Failed to notify passengers that flight was deleted '{result.FailReason}'!");
        }
    }
}