using Microsoft.Extensions.Configuration;

namespace AuditLog.API
{
    public class EventStoreConfiguration
    {
        public IConfiguration Configuration { get; }

        public EventStoreConfiguration(IConfiguration configuration)
            => Configuration = configuration;

        public string FlightStreamName => Configuration["EVENTSTORE_FLIGHT_STREAM_NAME"];
        public string PassengerStreamName => Configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];

    }
}