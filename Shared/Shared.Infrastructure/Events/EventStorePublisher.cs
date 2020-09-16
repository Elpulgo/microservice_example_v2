using System;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Shared.Core.Models;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Events
{
    public class EventStorePublisher<T> : IEventStorePublisher<T>
    {
        private readonly IEventStoreContext m_EventStoreContext;

        public EventStorePublisher(IEventStoreContext eventStoreContext)
        {
            m_EventStoreContext = eventStoreContext ?? throw new ArgumentNullException(nameof(eventStoreContext));
        }

        public async Task Publish(IEventData<T> eventData)
        {
            var streamName = m_EventStoreContext.EventStreamName;
            var eventType = eventData.MetaData.EventTypeOperation;
            var data = JsonSerializer.SerializeToUtf8Bytes<T>(eventData.Data);
            var metaData = JsonSerializer.SerializeToUtf8Bytes<IEventDataMeta>(eventData.MetaData);

            var eventPayload = new EventData(
                eventData.EventId,
                eventType.ToString(),
                isJson: true,
                data: data,
                metadata: metaData);

            var _result = await m_EventStoreContext.Connection.AppendToStreamAsync(
                streamName,
                ExpectedVersion.Any,
                eventPayload);
        }
    }
}