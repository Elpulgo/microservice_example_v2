using System.Reflection;
using Flights.Application;
using Flights.Core;
using Flights.Core.Extensions;
using Flights.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;

namespace Flights.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var eventstoreStreamName = Configuration["EVENTSTORE_STREAM_NAME"];
            var eventstoreConnection = Configuration["EVENTSTORE_CONNECTION"];
            var postgresConnection = Configuration["POSTGRE_CONNECTION"];

            services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgresConnection));
            services.AddSingleton<IEventStoreContext>(sp => new EventStoreContext(eventstoreConnection, eventstoreStreamName));

            services.AddTransient<IFlightReadRepository, FlightReadRepository>();
            services.AddTransient<IFlightEventStorePublisher, FlightEventStorePublisher>();

            services.AddMediatR(typeof(MediatRDependencyInjectionHelper).GetTypeInfo().Assembly);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add mapping between .Net entities and Postgresql columns, utilized in dapper.
            FluentMapperExtensions.Initialize();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
