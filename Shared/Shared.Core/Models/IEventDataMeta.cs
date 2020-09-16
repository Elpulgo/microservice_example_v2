using System;
using Shared.Core.Constants;

namespace Shared.Core.Models
{
    public interface IEventDataMeta
    {
        EventTypeOperation EventTypeOperation { get; }
        string EventName { get; }
        DateTime Timestamp { get; }
    }
}