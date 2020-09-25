using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.RPC;

namespace Passengers.Application.Queries
{

    public class FlightExistsQuery : IRequest<bool>
    {
        public Guid FlightId { get; set; }

        public FlightExistsQuery(Guid flightId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            FlightId = flightId;
        }
    }

    public class FlightExistsHandler
    : IRequestHandler<FlightExistsQuery, bool>
    {
        private readonly IFlightRpcClient m_FlightRpcClient;

        public FlightExistsHandler(IFlightRpcClient flightRpcClient)
            => m_FlightRpcClient = flightRpcClient ?? throw new ArgumentNullException(nameof(flightRpcClient));

        public async Task<bool> Handle(FlightExistsQuery request, CancellationToken cancellationToken)
            => await m_FlightRpcClient.FlightExistsAsync(request.FlightId);
    }
}