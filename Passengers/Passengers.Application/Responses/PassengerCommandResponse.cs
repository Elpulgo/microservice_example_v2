using System;
using Shared.Core.Models;

namespace Passengers.Application.Responses
{
    public class PassengerCommandResponse : CommandResponseBase
    {
        public Guid Id { get; set; }
        public PassengerCommandResponse(CommandResponseBase responseBase, Guid id)
        {
            Id = id;
            base.Error = responseBase.Error;
            base.StackTrace = responseBase.StackTrace;
            base.Success = responseBase.Success;
        }
    }
}