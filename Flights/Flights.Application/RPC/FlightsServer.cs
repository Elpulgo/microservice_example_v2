using System.Collections.Generic;
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
        {
            // TODO: Should implement a find instead. doing this to test the chain now..
            try
            {
                await m_FlightsReadRepository.GetByIdAsync(request.FlightId);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}