using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Passengers.Core;
using Passengers.Core.Extensions;
using Passengers.Infrastructure;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Events;

namespace Passengers.Processor
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
                    var eventstoreStreamName = hostContext.Configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];
                    var eventstoreConnection = hostContext.Configuration["EVENTSTORE_CONNECTION"];
                    var postgresConnection = hostContext.Configuration["POSTGRE_CONNECTION"];

                    services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgresConnection));
                    services.AddSingleton<IEventStoreContext>(sp => new EventStoreContext(eventstoreConnection, eventstoreStreamName));

                    services.AddSingleton<IPassengerWriteRepository, PassengerWriteRepository>();

                    services.AddSingleton<IPassengerEventStoreSubscriber>(sp => new PassengerEventStoreSubscriber(
                        sp.GetRequiredService<IEventStoreContext>(),
                        sp.GetRequiredService<IPassengerWriteRepository>(),
                        eventStoreGroupName));

                    services.AddHostedService<Worker>();
                });
    }
}
