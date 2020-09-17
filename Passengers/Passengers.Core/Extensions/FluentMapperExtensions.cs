using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Passengers.Core.Mapping;

namespace Passengers.Core.Extensions
{

    public static class FluentMapperExtensions
    {
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new PassengerMap());
                config.ForDommel();
            });
        }
    }
}