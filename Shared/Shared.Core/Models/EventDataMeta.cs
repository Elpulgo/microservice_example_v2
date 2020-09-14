using System;

namespace Shared.Core.Models
{
    public class EventDataMeta : IEventDataMeta
    {
        public string EventType { get; private set; }
        public string EventName { get; private set; }
        public DateTime Timestamp { get; private set; }

        public EventDataMeta(string eventType, string eventName)
        {
            Validate(eventType, eventName);

            EventType = eventType;
            EventName = eventName;
            Timestamp = DateTime.UtcNow;
        }

        private void Validate(string eventType, string eventName)
        {
            if (string.IsNullOrEmpty(eventType))
                throw new ArgumentNullException(nameof(eventType));

            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(eventName);
        }
    }
}