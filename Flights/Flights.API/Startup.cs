using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights.Core;
using Flights.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;

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

            var postgreConnectionString = "Server=db-flights;Port=5432;Database=flights;User Id=flights_user;Password=flights_user_pass;";


            services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgreConnectionString, "flights"));
            services.AddTransient<IFlightRepository, FlightRepository>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            // TEST
            var repo = app.ApplicationServices.GetRequiredService<IFlightRepository>();
            try
            {

                var result = repo.InsertAsync(new Flight()
                {
                    Id = new Shared.Core.TId()
                    {
                        Id = Guid.NewGuid()
                    },
                    Destination = "Copenhagen",
                    Origin = "Malmö",
                    FlightNumber = "SAS123",
                    Status = FlightStatus.Boarded
                }).Result;

            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"Exception, cant insert flight {ex.Message}");
            }
        }
    }
}
