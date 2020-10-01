using Flights.Application.RPC;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.RPC;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.RPC;

namespace Flights.API
{
    public static class ServiceRegistration
    {
        public static void AddPostgres(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var postgresConnection = configuration["POSTGRE_CONNECTION"];

            services.AddSingleton<PostgreContext>(sp =>
                new PostgreContext(postgresConnection));
        }

        public static void AddEventStore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var eventstoreStreamName = configuration["EVENTSTORE_FLIGHT_STREAM_NAME"];
            var eventstoreConnection = configuration["EVENTSTORE_CONNECTION"];

            services.AddSingleton<IEventStoreContext>(sp =>
                new EventStoreContext(
                    eventstoreConnection,
                    eventstoreStreamName));
        }
        public static void AddRPCServer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConnectionListenerFactory, SocketTransportFactory>();
            services.AddTransient<IFlightContract, FlightsServer>();

            var rpcServerPort = configuration["RPC_SERVER_PORT"];

            services.AddHostedService<StreamJsonRcpHost>(sp =>
                new StreamJsonRcpHost(
                    sp.GetRequiredService<IFlightContract>(),
                    sp.GetRequiredService<IConnectionListenerFactory>(),
                    int.Parse(rpcServerPort)));
        }

        public static void AddRPCClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var passengerRpcHostName = configuration["PASSENGER_RPC_HOSTNAME"];
            var passengerRpcPort = configuration["PASSENGER_RPC_PORT"];

            services.AddTransient<RpcClient>(sp => new RpcClient(
                passengerRpcHostName,
                int.Parse(passengerRpcPort)));

            services.AddTransient<IPassengerRpcClient, PassengerRpcClient>();
        }
    }
}