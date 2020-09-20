using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
