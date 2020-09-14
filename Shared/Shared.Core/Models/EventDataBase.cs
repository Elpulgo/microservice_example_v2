using System;

namespace Shared.Core.Models
{
    public class EventDataBase
    {
        public Guid EventId { get; }

        public EventDataBase()
        {
            EventId = Guid.NewGuid();
        }
    }
}