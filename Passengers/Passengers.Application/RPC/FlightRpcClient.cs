using System;
using System.Threading.Tasks;
using Shared.Core.RPC;
using Shared.Infrastructure.RPC;
using StreamJsonRpc;

namespace Passengers.Application.RPC
{
    public class FlightRpcClient : IFlightRpcClient
    {
        private readonly RpcClient m_RpcClient;

        public FlightRpcClient(RpcClient rpcClient)
        {
            m_RpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
        }

        public async Task<(bool Success, string FailReason)> AllPassengersBoardedAsync(Guid flightId)
        {
            using var handler = await m_RpcClient.GetMessageHandlerAsync();
            var jsonRpcClient = JsonRpc.Attach<IFlightContract>(handler);

            return await jsonRpcClient.AllPassengersBoardedAsync(new AllPassengersBoardedRequest() { FlightId = flightId });
        }

        public async Task<bool> FlightExistsAsync(Guid flightId)
        {
            using var handler = await m_RpcClient.GetMessageHandlerAsync();

            var jsonRpcClient = JsonRpc.Attach<IFlightContract>(handler);

            return await jsonRpcClient.FlightExistsAsync(new FlightExistsRequest() { FlightId = flightId });
        }
    }
}