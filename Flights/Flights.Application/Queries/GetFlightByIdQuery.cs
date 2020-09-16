using MediatR;
using System;
using Flights.Application.Responses;

namespace Flights.Application.Queries
{
    public class GetFlightByIdQuery : IRequest<FlightResponse>
    {
        public Guid Id { get; set; }

        public GetFlightByIdQuery(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (id == Guid.Empty)
                throw new ArgumentNullException($"{nameof(id)} can't be empty!");

            Id = id;
        }
    }
}