using EventStore.ClientAPI;

namespace Shared.Infrastructure.Data
{
    public interface IEventStoreContext
    {
        IEventStoreConnection Connection { get; }

        string EventStreamName { get; }

        EventStoreCredentials Credentials { get; }
    }
}