using System.Threading.Tasks;

namespace Shared.Core.RPC
{
    public interface IPassengerContract : IStreamJsonRpcServer
    {
        // TODO: Refactor into response class instead..
        Task<(bool Success, string FailReason)> FlightArrivedAsync(FlightActionRequest request);

        Task<(bool Success, string FailReason)> FlightDeletedAsync(FlightActionRequest request);
    }
}