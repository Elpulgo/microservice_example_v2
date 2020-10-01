using System;
using System.Threading.Tasks;

namespace Flights.Application.RPC
{
    public interface IPassengerRpcClient
    {
        Task<(bool Success, string FailReason)> FlightArrivedAsync(Guid flightId);
        Task<(bool Success, string FailReason)> FlightDeletedAsync(Guid flightId);
    }
}