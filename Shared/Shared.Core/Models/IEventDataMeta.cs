using System;

namespace Shared.Core.Models
{
    public interface IEventDataMeta
    {
        string EventType { get; }
        string EventName { get; }
        DateTime Timestamp { get; }
    }
}