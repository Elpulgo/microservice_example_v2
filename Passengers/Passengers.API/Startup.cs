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
            services.AddPostgres(Configuration);
            services.AddEventStore(Configuration);
            services.AddRPCClient(Configuration);
            services.AddRPCServer(Configuration);

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
    }
}
