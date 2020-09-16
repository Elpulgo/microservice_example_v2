using MediatR;
using System;
using Flights.Application.Responses;
using System.Collections.Generic;

namespace Flights.Application.Queries
{
    public class GetAllFlightsQuery : IRequest<IReadOnlyList<FlightResponse>>
    {
        public GetAllFlightsQuery()
        {
        }
    }
}