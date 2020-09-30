using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Core;
using Passengers.Core.Models;

namespace Passengers.Application.Queries
{

    public class AllPassengersBoardedQuery : IRequest<bool>
    {
        public Guid FlightId { get; set; }

        public Guid PassengerId { get; set; }

        public AllPassengersBoardedQuery(Guid flightId, Guid passengerId)
        {
            if (flightId == null)
                throw new ArgumentNullException(nameof(flightId));

            if (flightId == Guid.Empty)
                throw new ArgumentException($"{nameof(flightId)} can't be empty.");

            if (passengerId == null)
                throw new ArgumentNullException(nameof(passengerId));

            if (passengerId == Guid.Empty)
                throw new ArgumentException($"{nameof(passengerId)} can't be empty.");

            FlightId = flightId;
            PassengerId = passengerId;
        }
    }

    public class AllPassengersBoardedHandler
    : IRequestHandler<AllPassengersBoardedQuery, bool>
    {
        private readonly IPassengerReadRepository m_ReadRepository;

        public AllPassengersBoardedHandler(IPassengerReadRepository readRepository)
            => m_ReadRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));

        public async Task<bool> Handle(AllPassengersBoardedQuery request, CancellationToken cancellationToken)
            => await m_ReadRepository.HasAllPassengersBoarded(request.FlightId, request.PassengerId);
    }
}