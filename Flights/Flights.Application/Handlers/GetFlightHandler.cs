using System;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Queries;
using Flights.Application.Responses;
using Flights.Core;
using MediatR;

namespace Flights.Application.Handlers
{
    public class GetFlightHandler : IRequestHandler<GetFlightByIdQuery, FlightResponse>
    {
        private readonly IFlightReadRepository m_Repository;

        public GetFlightHandler(IFlightReadRepository repository)
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<FlightResponse> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
        {
            var flight = await m_Repository.GetByIdAsync(request.Id);

            if (flight == null)
                return null;

            return new FlightResponse()
            {
                Destination = flight.Destination,
                FlightNumber = flight.FlightNumber,
                Id = flight.Id,
                Origin = flight.Origin,
                Status = flight.Status
            };
        }
    }
}