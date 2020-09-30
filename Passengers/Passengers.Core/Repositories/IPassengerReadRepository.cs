using System;
using System.Threading.Tasks;
using Passengers.Core.Models;
using Shared.Infrastructure;

namespace Passengers.Core
{
    public interface IPassengerReadRepository : IReadRepository<Passenger>
    {
        Task<bool> HasAllPassengersBoarded(Guid flightId, Guid requestPassengerId);
    }
}