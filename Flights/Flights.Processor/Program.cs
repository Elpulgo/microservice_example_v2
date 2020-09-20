using Flights.Core;
using Flights.Core.Extensions;
using Flights.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;

namespace Flights.Processor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Add mapping between .Net entities and Postgresql columns, utilized in dapper.
            FluentMapperExtensions.Initialize();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var eventStoreGroupName = hostContext.Configuration["EVENTSTORE_SUBSCRIPTION_GROUP_NAME"];
                    var eventstoreStreamName = hostContext.Configuration["EVENTSTORE_FLIGHT_STREAM_NAME"];
                    var eventstoreConnection = hostContext.Configuration["EVENTSTORE_CONNECTION"];
                    var postgresConnection = hostContext.Configuration["POSTGRE_CONNECTION"];

                    services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgresConnection));
                    services.AddSingleton<IEventStoreContext>(sp => new EventStoreContext(eventstoreConnection, eventstoreStreamName));

                    services.AddSingleton<IFlightWriteRepository, FlightWriteRepository>();

                    services.AddSingleton<IFlightEventStoreSubscriber>(sp => new FlightEventStoreSubscriber(
                        sp.GetRequiredService<IEventStoreContext>(),
                        sp.GetRequiredService<IFlightWriteRepository>(),
                        eventStoreGroupName));

                    services.AddHostedService<Worker>();
                });
    }
}
