using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Queries;
using Flights.Application.Responses;
using Flights.Core;
using MediatR;

namespace Flights.Application.Handlers
{
    public class GetAllFlightsHandler : IRequestHandler<GetAllFlightsQuery, IReadOnlyList<FlightResponse>>
    {
        private readonly IFlightReadRepository m_Repository;

        public GetAllFlightsHandler(IFlightReadRepository repository)
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<FlightResponse>> Handle(GetAllFlightsQuery request, CancellationToken cancellationToken)
        {
            var allFlights = await m_Repository.GetAllAsync();

            return allFlights
                .Select(flight => new FlightResponse()
                {
                    Destination = flight.Destination,
                    FlightNumber = flight.FlightNumber,
                    Id = flight.Id,
                    Origin = flight.Origin,
                    Status = flight.Status
                })
                .ToList();
        }
    }
}