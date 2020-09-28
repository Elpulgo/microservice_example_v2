using System;
using System.Threading.Tasks;

namespace Passengers.Application.RPC
{
    public interface IFlightRpcClient
    {
        Task<bool> FlightExistsAsync(Guid flightId);

        Task AllPassengersBoardedAsync(Guid flightId);
    }
}