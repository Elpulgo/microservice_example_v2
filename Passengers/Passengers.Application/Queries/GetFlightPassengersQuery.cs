using System;
using System.Collections.Generic;
using MediatR;
using Passengers.Application.Responses;

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
}