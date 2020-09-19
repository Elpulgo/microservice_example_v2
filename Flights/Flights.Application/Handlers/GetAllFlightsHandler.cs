using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Mapper;
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

            return allFlights.Map();
        }
    }
}