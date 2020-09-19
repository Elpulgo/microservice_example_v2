using MediatR;
using Flights.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Flights.Core;
using System.Threading;
using Flights.Application.Mapper;

namespace Flights.Application.Queries
{
    public class GetAllFlightsQuery : IRequest<IReadOnlyList<FlightResponse>>
    {
        public GetAllFlightsQuery()
        { }
    }

    public class GetAllFlightsHandler
        : IRequestHandler<GetAllFlightsQuery, IReadOnlyList<FlightResponse>>
    {
        private readonly IFlightReadRepository m_Repository;

        public GetAllFlightsHandler(IFlightReadRepository repository)
            => m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<IReadOnlyList<FlightResponse>> Handle(GetAllFlightsQuery request, CancellationToken cancellationToken)
            => (await m_Repository.GetAllAsync()).Map();
    }
}