using System;
using MediatR;
using Passengers.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Passengers.Core;
using Passengers.Application.Mapper;

namespace Passengers.Application.Queries
{
    public class GetPassengerByIdQuery : IRequest<PassengerResponse>
    {
        public Guid Id { get; set; }
        public GetPassengerByIdQuery(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (id == Guid.Empty)
                throw new ArgumentNullException($"{nameof(id)} can't be empty!");

            Id = id;
        }
    }

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

            return passenger.Map();
        }
    }
}