using System;
using Shared.Core.Constants;

namespace Shared.Core.Models
{
    public class EventDataMeta : IEventDataMeta
    {
        public EventTypeOperation EventTypeOperation { get; private set; }
        public string EventName { get; private set; }
        public DateTime Timestamp { get; private set; }

        public EventDataMeta(EventTypeOperation eventTypeOperation, string eventName)
        {
            Validate(eventTypeOperation, eventName);

            EventTypeOperation = eventTypeOperation;
            EventName = eventName;
            Timestamp = DateTime.UtcNow;
        }

        private void Validate(EventTypeOperation eventTypeOperation, string eventName)
        {
            if (eventTypeOperation == EventTypeOperation.None)
                throw new ArgumentException($"{nameof(eventTypeOperation)} can't be None.");

            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(eventName);
        }
    }
}