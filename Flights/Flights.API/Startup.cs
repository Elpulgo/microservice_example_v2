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
            services.AddEventStore(Configuration);
            services.AddPostgres(Configuration);
            services.AddRPCClient(Configuration);
            services.AddRPCServer(Configuration);

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
