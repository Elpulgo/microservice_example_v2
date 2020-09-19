using MediatR;
using System;
using Flights.Application.Responses;
using Flights.Core;
using System.Threading.Tasks;
using System.Threading;
using Flights.Application.Mapper;

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

    public class GetFlightHandler : IRequestHandler<GetFlightByIdQuery, FlightResponse>
    {
        private readonly IFlightReadRepository m_Repository;

        public GetFlightHandler(IFlightReadRepository repository)
            => m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<FlightResponse> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
            => (await m_Repository.GetByIdAsync(request.Id))?.Map();
    }
}