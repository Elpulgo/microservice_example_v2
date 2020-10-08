using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Events
{
    public class EventStoreReader : IEventStoreReader
    {
        private readonly IEventStoreContext m_Context;

        public EventStoreReader(IEventStoreContext context)
            => m_Context = context ?? throw new System.ArgumentNullException(nameof(context));

        public async Task<IReadOnlyList<ResolvedEvent>> ReadAll(string streamName)
        {
            var events = new List<ResolvedEvent>();

            if (string.IsNullOrEmpty(streamName))
                return events;

            StreamEventsSlice currentSlice;
            long nextSliceStart = 0;

            do
            {
                currentSlice = await m_Context.Connection.ReadStreamEventsForwardAsync(
                    streamName,
                    nextSliceStart,
                    100,
                    false,
                    new UserCredentials(m_Context.Credentials.User, m_Context.Credentials.Password));

                nextSliceStart = currentSlice.NextEventNumber;

                events.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return events;
        }
    }
}