using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Mapper;
using Passengers.Application.Responses;
using Passengers.Core;

namespace Passengers.Application.Queries
{
    public class GetFlightPassengersQuery : IRequest<IReadOnlyList<PassengerResponse>>
    {
        public Guid FlightId { get; }
        public GetFlightPassengersQuery(Guid flightId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            FlightId = flightId;
        }
    }

    public class GetFlightPassengersHandler
    : IRequestHandler<GetFlightPassengersQuery, IReadOnlyList<PassengerResponse>>
    {
        private readonly IPassengerReadRepository m_Repository;

        public GetFlightPassengersHandler(IPassengerReadRepository repository)
            => m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<IReadOnlyList<PassengerResponse>> Handle(GetFlightPassengersQuery request, CancellationToken cancellationToken)
            => (await m_Repository
                .SelectAsync(passenger => passenger.FlightId == request.FlightId)).Map();
    }
}