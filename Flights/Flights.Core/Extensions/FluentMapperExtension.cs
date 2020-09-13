using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Flights.Core.Mapping;

namespace Flights.Core.Extensions
{
    public static class FluentMapperExtensions
    {
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new FlightMap());
                config.ForDommel();
            });
        }
    }
}