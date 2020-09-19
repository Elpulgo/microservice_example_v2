using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Queries;
using Passengers.Application.Responses;
using Passengers.Core;

namespace Passengers.Application.Handlers
{
    public class GetPassengerByIdHandler
        : IRequestHandler<GetPassengerByIdQuery, PassengerResponse>
    {
        private readonly IPassengerReadRepository m_Repository;

        public GetPassengerByIdHandler(IPassengerReadRepository repository)
        {
            m_Repository = repository;
        }

        public async Task<PassengerResponse> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
        {
            var passenger = await m_Repository.GetByIdAsync(request.Id);

            if (passenger == null)
                return null;

            return new PassengerResponse()
            {
                FlightId = passenger.FlightId,
                Id = passenger.Id,
                Name = passenger.Name,
                Status = passenger.Status
            };
        }
    }
}