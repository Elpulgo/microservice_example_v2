using System.Threading;
using System.Threading.Tasks;
using Flights.Application.Commands;
using Flights.Application.Responses;
using MediatR;

namespace Flights.Application.Handlers
{
    public class CreateFlightHandler : IRequestHandler<CreateFlightCommand, FlightCommandResponse>
    {
        public Task<FlightCommandResponse> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {

            // TODO: Use eventstore to publish event here.. need new Flight.Infrastructure something..?
            // Subscriber/Publisher or something similar, which is generic?
            throw new System.NotImplementedException();
        }
    }
}