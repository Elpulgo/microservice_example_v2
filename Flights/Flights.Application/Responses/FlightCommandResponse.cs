using System;
using Shared.Core.Models;

namespace Flights.Application.Responses
{
    public class FlightCommandResponse : CommandResponseBase
    {
        public Guid Id { get; set; }

        public FlightCommandResponse(CommandResponseBase responseBase, Guid id)
        {
            Id = id;
            base.Success = responseBase.Success;
            base.StackTrace = responseBase.StackTrace;
            base.Error = responseBase.Error;
        }
    }
}