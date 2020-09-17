using System;
using MediatR;
using Passengers.Application.Responses;

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
}