using System.Collections.Generic;
using MediatR;
using Passengers.Application.Responses;

namespace Passengers.Application.Queries
{
    public class GetFlightPassengersQuery : IRequest<IReadOnlyList<PassengerResponse>>
    {
        public GetFlightPassengersQuery()
        {

        }
    }
}