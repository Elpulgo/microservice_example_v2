using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Passengers.Core;
using Passengers.Core.Models;
using Shared.Infrastructure;

namespace Passengers.Infrastructure
{
    public class PassengerReadRepository : ReadRepository<Passenger>, IPassengerReadRepository
    {
        private readonly PostgreContext m_Context;

        public PassengerReadRepository(PostgreContext context) : base(context, Passenger.Table)
            => m_Context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<bool> HasAllPassengersBoarded(Guid flightId, Guid requestPassengerId)
        {
            base.EnsureNotNullOrEmpty(flightId);
            base.EnsureNotNullOrEmpty(requestPassengerId);

            using var connection = m_Context.Instance;

            var result = await connection.QueryAsync<int>(
                $@"SELECT 
                        status
                        FROM {Passenger.Table}
                        WHERE 
                            flight_id=@FlightId 
                            AND 
                            id != @PassengerId
                        GROUP BY status",
                new
                {
                    FlightId = flightId,
                    PassengerId = requestPassengerId
                });


            if (result.Count() == 1 && ((PassengerStatus)result.Single()) == PassengerStatus.Boarded)
                return true;

            return false;
        }

        public async Task<IReadOnlyList<Passenger>> GetAllPassengersOnFlightAsync(Guid flightId)
        {
            base.EnsureNotNullOrEmpty(flightId);

            using var connection = m_Context.Instance;

            var result = await connection.QueryAsync<Passenger>(
                $@"
                    SELECT * FROM {Passenger.Table}
                    WHERE flight_id=@FlightId
                ",
                new
                {
                    FlightId = flightId
                }
            );

            return result.ToList();
        }
    }
}