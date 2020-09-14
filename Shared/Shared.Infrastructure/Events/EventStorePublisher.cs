using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Events
{
    public class EventStorePublisher<T> : IEventStorePublisher<T>
    {
        private readonly IEventStoreContext m_EventStoreContext;

        public EventStorePublisher(IEventStoreContext eventStoreContext)
        {
            m_EventStoreContext = eventStoreContext;
        }

        public async Task Publish(T @event)
        {
            var streamName = m_EventStoreContext.EventStreamName;
            var eventType = "some-type";
            var data = JsonSerializer.SerializeToUtf8Bytes<T>(@event);
            var metaData = "some-metadata";

            var eventPayload = new EventData(
                Guid.NewGuid(),
                eventType,
                isJson: true,
                data: data,
                metadata: Encoding.UTF8.GetBytes(metaData));

            var result = await m_EventStoreContext.Connection.AppendToStreamAsync(
                streamName,
                ExpectedVersion.Any,
                eventPayload);
        }
    }
}