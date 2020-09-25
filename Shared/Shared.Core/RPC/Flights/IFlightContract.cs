using System.Threading.Tasks;

namespace Shared.Core.RPC
{
    public interface IFlightContract : IStreamJsonRpcServer
    {
        Task<bool> FlightExistsAsync(FlightExistsRequest request);

        Task AllPassengersBoardedAsync();
    }
}