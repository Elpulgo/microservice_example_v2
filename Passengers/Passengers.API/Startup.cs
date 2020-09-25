using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passengers.Application;
using Passengers.Application.RPC;
using Passengers.Core;
using Passengers.Core.Extensions;
using Passengers.Infrastructure;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.RPC;

namespace Passengers.API
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
            ConfigurePostgres(services);
            ConfigureEventStore(services);
            ConfigureRPC(services);

            services.AddTransient<IPassengerReadRepository, PassengerReadRepository>();
            services.AddTransient<IPassengerEventStorePublisher, PassengerEventStorePublisher>();

            services.AddMediatR(typeof(MediatRDependencyInjectionHelper).GetTypeInfo().Assembly);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add mapping between .Net entities and Postgresql columns, utilized in dapper.
            FluentMapperExtensions.Initialize();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigurePostgres(IServiceCollection services)
        {
            var postgresConnection = Configuration["POSTGRE_CONNECTION"];

            services.AddSingleton<PostgreContext>(sp =>
                new PostgreContext(postgresConnection));
        }

        private void ConfigureEventStore(IServiceCollection services)
        {
            var eventstoreStreamName = Configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];
            var eventstoreConnection = Configuration["EVENTSTORE_CONNECTION"];

            services.AddSingleton<IEventStoreContext>(sp =>
                new EventStoreContext(
                    eventstoreConnection,
                    eventstoreStreamName));
        }

        private void ConfigureRPC(IServiceCollection services)
        {
            var flightRpcHostName = Configuration["FLIGHT_RPC_HOSTNAME"];
            var flightRpcPort = Configuration["FLIGHT_RPC_PORT"];

            services.AddTransient<RpcClient>(sp => new RpcClient(
                flightRpcHostName,
                int.Parse(flightRpcPort)));

            services.AddTransient<IFlightRpcClient, FlightRpcClient>();
        }
    }
}
