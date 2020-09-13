using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights.Core;
using Flights.Core.Extensions;
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

            var postgreConnectionString = "Server=db-flights;Port=5432;Database=Flights;User Id=flights_user;Password=flights_user_pass;";


            services.AddSingleton<PostgreContext>(sp => new PostgreContext(postgreConnectionString));
            services.AddTransient<IFlightReadRepository, FlightReadRepository>();
            services.AddTransient<IFlightWriteRepository, FlightWriteRepository>();

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


            // Test(app).Wait();
            Test2(app).Wait();

        }

        private async Task Test2(IApplicationBuilder app)
        {
            // TEST
            var repoRead = app.ApplicationServices.GetRequiredService<IFlightReadRepository>();
            var repoWrite = app.ApplicationServices.GetRequiredService<IFlightWriteRepository>();

            var result = await repoRead.SelectAsync((a => a.FlightNumber == "BA_4444"));

            foreach (var item in result)
            {
                Console.WriteLine($"Flight: {item.Id} | {item.Destination} | {item.Origin} | {item.Status} | {item.FlightNumber}");
            }
        }

        private async Task Test(IApplicationBuilder app)
        {
            // TEST
            var repoRead = app.ApplicationServices.GetRequiredService<IFlightReadRepository>();
            var repoWrite = app.ApplicationServices.GetRequiredService<IFlightWriteRepository>();

            try
            {

                // System.Threading.Thread.Sleep(10000);
                // Console.WriteLine("Slepts 10 secs.. will try");

                var result = await repoWrite.InsertAsync(new Flight(
                    "Malmö_111",
                    "Copenhagen_111",
                    "SAS123_111",
                    FlightStatus.Boarded)
                );

                var result2 = await repoWrite.InsertAsync(new Flight(
                    "Malmö_222",
                    "Copenhagen_222",
                    "SAS123_222",
                    FlightStatus.Boarded)
                );

                var result3 = await repoWrite.InsertAsync(new Flight(
                    "Malmö_333",
                    "Copenhagen_333",
                    "SAS123_333",
                    FlightStatus.Boarded)
                );

                Console.WriteLine($"Successfully added 3 flights!");

                var flight = await repoRead.GetByIdAsync(result.Id);
                Console.WriteLine($"Successfully fetched flight with id: {flight.Id} and flight number {flight.FlightNumber}");

                var allFlights = await repoRead.GetAllAsync();

                foreach (var f in allFlights)
                {
                    Console.WriteLine($"Got flight with id: {f.Id} and flightnumber: {f.FlightNumber}");
                }

                var idToDelete = result.Id;

                await repoWrite.DeleteByIdAsync(result.Id);

                Console.WriteLine($"Successfully deleted flight with id: {idToDelete}");

                var idToDelete3 = result3.Id;

                await repoWrite.DeleteAsync(result3);
                Console.WriteLine($"Successfully deleted flight with id: {idToDelete3} by deleting entity..");

                result2.FlightNumber = "BA_4444";
                var success = await repoWrite.UpdateAsync(result2);

                Console.WriteLine($"Updated flight? {success}");

                Console.WriteLine($"################\n");


                allFlights = await repoRead.GetAllAsync();

                foreach (var f in allFlights)
                {
                    Console.WriteLine($"Final flight with id: {f.Id} and flightnumber: {f.FlightNumber}");
                }
            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"Exception, cant insert flight {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
