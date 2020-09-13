
using Dapper.FluentMap.Dommel.Mapping;

namespace Flights.Core.Mapping
{
    public class FlightMap : DommelEntityMap<Flight>
    {
        public FlightMap()
        {
            ToTable("flights");

            Map(m => m.Id).ToColumn("id", false);
            Map(m => m.Destination).ToColumn("destination", false);
            Map(m => m.FlightNumber).ToColumn("flight_number", false);
            Map(m => m.Origin).ToColumn("origin", false);
            Map(m => m.Status).ToColumn("status", false);
        }
    }
}