using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passengers.Application;
using Passengers.Core;
using Passengers.Core.Extensions;
using Passengers.Infrastructure;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;

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
            var eventstoreStreamName = Configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];
            var eventstoreConnection = Configuration["EVENTSTORE_CONNECTION"];
            var postgresConnection = Configuration["POSTGRE_CONNECTION"];

            services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgresConnection));
            services.AddSingleton<IEventStoreContext>(sp => new EventStoreContext(eventstoreConnection, eventstoreStreamName));

            services.AddTransient<IPassengerReadRepository, PassengerReadRepository>();
            services.AddTransient<IPassengerEventStorePublisher, PassengerEventStorePublisher>();

            services.AddSingleton<RpcClient>();


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

            System.Console.WriteLine("fetching RpcClient and sending..");
            var client = app.ApplicationServices.GetRequiredService<RpcClient>();
            client.Send().Wait();
            System.Console.WriteLine("Done sending..");
        }
    }
}
