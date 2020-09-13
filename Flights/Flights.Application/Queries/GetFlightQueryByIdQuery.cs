using MediatR;
using System;
using Flights.Application.Responses;

namespace Flights.Application.Queries
{
    public class GetFlightQueryByIdQuery : IRequest<FlightResponse>
    {
        public Guid Id { get; set; }

        public GetFlightQueryByIdQuery(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (id == Guid.Empty)
                throw new ArgumentNullException($"{nameof(id)} can't be empty!");

            Id = id;
        }
    }
}