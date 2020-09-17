using Dapper.FluentMap.Dommel.Mapping;
using Passengers.Core.Models;

namespace Passengers.Core.Mapping
{
    public class PassengerMap : DommelEntityMap<Passenger>
    {
        public PassengerMap()
        {
            ToTable("passengers");

            Map(m => m.Id).ToColumn("id", false);
            Map(m => m.FlightId).ToColumn("flight_id", false);
            Map(m => m.Name).ToColumn("name", false);
            Map(m => m.Status).ToColumn("status", false);
        }
    }
}