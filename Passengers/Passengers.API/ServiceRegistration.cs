using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passengers.Application.RPC;
using Shared.Core.RPC;
using Shared.Infrastructure;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.RPC;

namespace Passengers.API
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
            var eventstoreStreamName = configuration["EVENTSTORE_PASSENGER_STREAM_NAME"];
            var eventstoreConnection = configuration["EVENTSTORE_CONNECTION"];

            services.AddSingleton<IEventStoreContext>(sp =>
                new EventStoreContext(
                    eventstoreConnection,
                    eventstoreStreamName));
        }

        public static void AddRPCClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var flightRpcHostName = configuration["FLIGHT_RPC_HOSTNAME"];
            var flightRpcPort = configuration["FLIGHT_RPC_PORT"];

            services.AddTransient<RpcClient>(sp => new RpcClient(
                flightRpcHostName,
                int.Parse(flightRpcPort)));

            services.AddTransient<IFlightRpcClient, FlightRpcClient>();
        }

        public static void AddRPCServer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConnectionListenerFactory, SocketTransportFactory>();
            services.AddTransient<IPassengerContract, PassengersServer>();

            var rpcServerPort = configuration["RPC_SERVER_PORT"];

            services.AddHostedService<StreamJsonRcpHost>(sp =>
                new StreamJsonRcpHost(
                    sp.GetRequiredService<IPassengerContract>(),
                    sp.GetRequiredService<IConnectionListenerFactory>(),
                    int.Parse(rpcServerPort)));
        }
    }
}