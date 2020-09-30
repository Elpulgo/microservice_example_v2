using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Passengers.Application.Commands;
using Passengers.Core;
using Passengers.Core.Models;
using Shared.Core.RPC;

namespace Passengers.Application.RPC
{
    public class PassengersServer : IPassengerContract
    {
        private readonly IMediator m_Mediator;
        private readonly IPassengerReadRepository m_ReadRepository;

        public PassengersServer(
            IMediator mediator,
            IPassengerReadRepository readRepository)
        {
            m_Mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            m_ReadRepository = readRepository ?? throw new System.ArgumentNullException(nameof(readRepository));
        }

        public async Task<(bool Success, string FailReason)> FlightArrivedAsync(FlightActionRequest request)
        {
            var passengers = await m_ReadRepository.GetAllPassengersOnFlightAsync(request.FlightId);

            if (!passengers.Any())
                return (true, string.Empty);

            try
            {
                foreach (var passenger in passengers)
                {
                    await m_Mediator.Send(new UpdatePassengerCommand()
                    {
                        FlightId = passenger.FlightId,
                        Id = passenger.Id,
                        Name = passenger.Name,
                        Status = PassengerStatus.Arrived
                    });
                }

                return (true, string.Empty);
            }
            catch (Exception exception)
            {
                return (false, exception.Message);
            }
        }

        public async Task<(bool Success, string FailReason)> FlightDeletedAsync(FlightActionRequest request)
        {
            var passengers = await m_ReadRepository.GetAllPassengersOnFlightAsync(request.FlightId);

            if (!passengers.Any())
                return (true, string.Empty);

            try
            {
                foreach (var passenger in passengers)
                {
                    await m_Mediator.Send(new DeletePassengerCommand() { Id = passenger.Id });
                }

                return (true, string.Empty);
            }
            catch (Exception exception)
            {
                return (false, exception.Message);
            }
        }
    }
}