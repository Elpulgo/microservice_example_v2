using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flights;
using Grpc.Net.Client;

namespace Passengers.API
{
    public class RpcClient
    {
        public RpcClient()
        {

        }

        public async Task Send()
        {
            try
            {

                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                using var channel = GrpcChannel.ForAddress("https://flights-api:8041");
                var client = new FlightService.FlightServiceClient(channel);


                var reply = await client.FlightExistsAsync(new FlightExistsRequest() { FlightId = "this is a flight id" });
                Console.WriteLine($"Reply: '{reply.Exists}'");
            }
            catch (System.Exception exception)
            {
                Console.WriteLine($"Failed with calling flight exists: '{exception.Message}', {exception.StackTrace}");
            }
        }
    }
}