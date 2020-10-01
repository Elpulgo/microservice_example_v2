using System;
using System.Threading.Tasks;
using Shared.Core.RPC;
using Shared.Infrastructure.RPC;
using StreamJsonRpc;

namespace Flights.Application.RPC
{
    public class PassengerRpcClient : IPassengerRpcClient
    {
        private readonly RpcClient m_RpcClient;

        public PassengerRpcClient(RpcClient rpcClient)
            => m_RpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));

        public async Task<(bool Success, string FailReason)> FlightArrivedAsync(Guid flightId)
        {
            using var handler = await m_RpcClient.GetMessageHandlerAsync();
            var jsonRpcClient = JsonRpc.Attach<IPassengerContract>(handler);

            return await jsonRpcClient.FlightArrivedAsync(new FlightActionRequest { FlightId = flightId });
        }

        public async Task<(bool Success, string FailReason)> FlightDeletedAsync(Guid flightId)
        {
            using var handler = await m_RpcClient.GetMessageHandlerAsync();
            var jsonRpcClient = JsonRpc.Attach<IPassengerContract>(handler);

            return await jsonRpcClient.FlightDeletedAsync(new FlightActionRequest { FlightId = flightId });
        }
    }
}