using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.RPC;
using MediatR;

namespace Flights.Application.Notifications
{
    public class FlightArrivedNotification : INotification
    {
        public Guid FlightId { get; set; }

        public FlightArrivedNotification(Guid flightId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            FlightId = flightId;
        }
    }

    public class NotifyPassengersFlightArrived
    : INotificationHandler<FlightArrivedNotification>
    {
        private readonly IPassengerRpcClient m_PassengerRpcClient;

        public NotifyPassengersFlightArrived(IPassengerRpcClient passengerRpcClient)
            => m_PassengerRpcClient = passengerRpcClient ?? throw new ArgumentNullException(nameof(passengerRpcClient));

        public async Task Handle(FlightArrivedNotification notification, CancellationToken cancellationToken)
        {
            var result = await m_PassengerRpcClient.FlightArrivedAsync(notification.FlightId);
            if (result.Success)
                return;

            Console.WriteLine($"Failed to notify passengers that flight arrived! '{result.FailReason}'");
        }
    }
}