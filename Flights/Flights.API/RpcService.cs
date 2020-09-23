using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Flights.API
{

    public class RpcService: FlightService.FlightServiceBase
    {
        public RpcService()
        {

        }

        public override Task<FlightExistsReply> FlightExists(FlightExistsRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Request from '{request.FlightId}'");
            return Task.FromResult(new FlightExistsReply() { Exists = true });
        }
    }
}