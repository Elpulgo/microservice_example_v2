using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Shared.Infrastructure.Events
{
    public interface IEventStoreReader
    {
        Task<IReadOnlyList<ResolvedEvent>> ReadAll(string streamName);
    }
}