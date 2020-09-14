using System;

namespace Shared.Core.Models
{

    public interface IEventData<T>
    {
        Guid EventId { get; }
        T Data { get; }
        IEventDataMeta MetaData { get; }
    }
}