using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Queries;
using Passengers.Application.Responses;
using Passengers.Core;

namespace Passengers.Application.Handlers
{
    public class GetFlightPassengersHandler
    : IRequestHandler<GetFlightPassengersQuery, IReadOnlyList<PassengerResponse>>
    {
        private readonly IPassengerReadRepository m_Repository;

        public GetFlightPassengersHandler(IPassengerReadRepository repository)
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<PassengerResponse>> Handle(GetFlightPassengersQuery request, CancellationToken cancellationToken)
        {
            var passengers = await m_Repository
                .SelectAsync(passenger => passenger.FlightId == request.FlightId);

            return passengers.Map();
        }
    }
}