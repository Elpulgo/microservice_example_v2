using System.Threading.Tasks;
using Flights.Core;
using Shared.Core.RPC;

namespace Flights.Application.RPC
{
    public class FlightsServer : IFlightContract
    {
        private readonly IFlightReadRepository m_FlightsReadRepository;

        public FlightsServer(IFlightReadRepository flightsReadRepository)
        {
            m_FlightsReadRepository = flightsReadRepository ?? throw new System.ArgumentNullException(nameof(flightsReadRepository));
        }

        public Task AllPassengersBoardedAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> FlightExistsAsync(FlightExistsRequest request)
            => await m_FlightsReadRepository.ExistsAsync(request.FlightId);
    }
}