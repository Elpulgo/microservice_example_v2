using System;
using Shared.Core.Models;

namespace AuditLog.API.Models
{
    public interface IAuditLogModel<T>
    {
        DateTime Created { get; }
        string EventType { get; }
        long EventNumber { get; }
        Guid EventId { get; }
        T Data { get; }
        EventDataMeta MetaData { get; }
    }
}