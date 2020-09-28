using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Core;
using MediatR;
using Shared.Core.RPC;

namespace Flights.Application.RPC
{
    public class FlightsServer : IFlightContract
    {
        private readonly IFlightReadRepository m_FlightsReadRepository;
        private readonly IMediator m_Mediator;

        public FlightsServer(
            IFlightReadRepository flightsReadRepository,
            IMediator mediator)
        {
            m_FlightsReadRepository = flightsReadRepository ?? throw new System.ArgumentNullException(nameof(flightsReadRepository));
            m_Mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task AllPassengersBoardedAsync(AllPassengersBoardedRequest request)
        {
            var flight = await m_FlightsReadRepository.GetByIdAsync(request.FlightId);

            var updateCommand = new UpdateFlightCommand()
            {
                Destination = flight.Destination,
                FlightNumber = flight.FlightNumber,
                Id = flight.Id,
                Origin = flight.Origin,
                Status = FlightStatus.AllBoarded
            };

            await m_Mediator.Send(updateCommand);
        }

        public async Task<bool> FlightExistsAsync(FlightExistsRequest request)
            => await m_FlightsReadRepository.ExistsAsync(request.FlightId);
    }
}