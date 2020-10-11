using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Shared.Core.Constants;
using Shared.Infrastructure.Data;
using System.Text.Json;
using EventStore.ClientAPI.Common;
using System.Threading;

namespace Shared.Infrastructure.Events
{
    public class EventStoreSubscriber<T> : IEventStoreSubscriber<T>
    {
        private const int MaxConnectionRetries = 10;
        private readonly IEventStoreContext m_Context;
        private readonly IWriteRepository<T> m_WriteRepository;
        private readonly string m_GroupName;
        private ProcessedEventCountHandler ProcessedEventCountHandler => ProcessedEventCountHandler.Instance;

        public EventStoreSubscriber(
            IEventStoreContext context,
            IWriteRepository<T> writeRepository,
            string groupName)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            m_WriteRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            m_GroupName = string.IsNullOrEmpty(groupName) ? throw new ArgumentNullException(nameof(groupName)) : groupName;
        }

        public async Task Subscribe()
        {
            var settings = CreateSettings();
            await CreatePersistentSubscriptionIfNotExistsAsync(settings);
            await ReplayEventsUpUntilCurrentStateAsync(settings);

            try
            {
                await m_Context.Connection.ConnectToPersistentSubscriptionAsync(
                    stream: m_Context.EventStreamName,
                    m_GroupName,
                    EventAppeardAction(),
                    subscriptionDropped: null,
                    userCredentials: new UserCredentials(
                        m_Context.Credentials.User,
                        m_Context.Credentials.Password),
                    bufferSize: 10,
                    autoAck: false);

                Console.WriteLine($"Successfully attached to persistent subscription stream: '{m_Context.EventStreamName}', groupname: '{m_GroupName}'.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to attach to persistent subscription stream: '{m_Context.EventStreamName}', groupname: '{m_GroupName}', reason: {exception.Message}");
            }

            Action<EventStorePersistentSubscriptionBase, ResolvedEvent> EventAppeardAction()
            {
                return async (subscription, evt) =>
                {
                    // Skip event if we have already processed it
                    if (ProcessedEventCountHandler.ReadNumberOfProcessedEvents() >= evt.Event.EventNumber)
                    {
                        subscription.Acknowledge(evt);
                        return;
                    }

                    var (success, failReason) = await ExecuteAction(evt);

                    if (success)
                        subscription.Acknowledge(evt);
                    else
                        subscription.Fail(
                            @event: evt,
                            action: PersistentSubscriptionNakEventAction.Skip,
                            reason: failReason);
                };
            }
        }

        private async Task ReplayEventsUpUntilCurrentStateAsync(PersistentSubscriptionSettings settings)
        {
            Console.WriteLine("Will replay events which have been missed.");

            StreamEventsSlice streamEventsSlice = null;
            int batchSize = 50;

            do
            {
                var lastProcessedEvent = ProcessedEventCountHandler.ReadNumberOfProcessedEvents();

                streamEventsSlice = await m_Context.Connection.ReadStreamEventsForwardAsync(
                    stream: m_Context.EventStreamName,
                    start: lastProcessedEvent,
                    count: batchSize,
                    resolveLinkTos: true,
                    userCredentials: new UserCredentials(
                            m_Context.Credentials.User,
                            m_Context.Credentials.Password)
                    );

                foreach (var evt in streamEventsSlice.Events)
                {
                    var (success, failReason) = await ExecuteAction(evt);

                    if (success)
                        Console.WriteLine($"Successfully persisted event # '{evt.OriginalEventNumber}' when replaying missed events!");
                    else

                        Console.WriteLine($"Failed to persist event # '{evt.OriginalEventNumber}'. Event will be discarded since it can't be handled.");
                }
            } while (!streamEventsSlice.IsEndOfStream);

            Console.WriteLine("Finished replaying events!");
        }

        private async Task<(bool Success, string FailReason)> ExecuteAction(ResolvedEvent evt)
        {
            var utf8EncodedData = Encoding.UTF8.GetString(evt.Event.Data);
            var entity = JsonSerializer.Deserialize<T>(utf8EncodedData);
            var operation = ParseEventTypeOperation(evt.Event.EventType);

            Console.WriteLine($"Got an event with operation '{operation.ToString()}', '{utf8EncodedData}'");

            try
            {
                switch (operation)
                {
                    case EventTypeOperation.Create:
                        await m_WriteRepository.InsertAsync(entity);
                        break;
                    case EventTypeOperation.Update:
                        var result = await m_WriteRepository.UpdateAsync(entity);
                        if (!result)
                        {
                            Console.WriteLine($"Failed to update entity: {utf8EncodedData}");
                        }
                        break;
                    case EventTypeOperation.Delete:
                        await m_WriteRepository.DeleteAsync(entity);
                        break;
                    default:
                        Console.WriteLine($"{operation.ToString()} not allowed in this context.");
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed with operation '{operation.ToString()}' for event with data: {utf8EncodedData}, caused by {exception.Message}");
                return (false, $"Failed with operation '{operation.ToString()}' for event with data: {utf8EncodedData}, caused by {exception.Message}");
            }

            ProcessedEventCountHandler.PersistNumberOfProcessedEvents(evt.Event.EventNumber);
            return (true, string.Empty);
        }

        private async Task CreatePersistentSubscriptionIfNotExistsAsync(PersistentSubscriptionSettings settings)
        {
            bool success = false;
            int retries = 0;

            do
            {
                retries++;

                try
                {
                    await m_Context.Connection.CreatePersistentSubscriptionAsync(
                        m_Context.EventStreamName,
                        m_GroupName,
                        settings,
                        new UserCredentials(
                            m_Context.Credentials.User,
                            m_Context.Credentials.Password));

                    success = true;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Group with name '{m_GroupName}' already exists, won't create a new persistant subscription, but attach to existing.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Failed to create persistent subscription for '{m_GroupName}'. {exception.Message}.");
                }
                finally
                {
                    if (!success || retries < MaxConnectionRetries)
                    {
                        Console.WriteLine($"Failed to create persistant subscription, try {retries} / {MaxConnectionRetries}...");
                        Thread.Sleep(1000);
                    }
                }
            } while (!success || retries < MaxConnectionRetries);
        }

        private PersistentSubscriptionSettings CreateSettings()
         => PersistentSubscriptionSettings
                .Create()
                .DoNotResolveLinkTos()
                .WithNamedConsumerStrategy(SystemConsumerStrategies.DispatchToSingle)
                .StartFrom(ProcessedEventCountHandler.ReadNumberOfProcessedEvents())
                .Build();

        private static EventTypeOperation ParseEventTypeOperation(string input)
        {
            if (!Enum.TryParse<EventTypeOperation>(input, true, out EventTypeOperation operation))
                throw new ArgumentException($"Failed to convert '{input}' to EventTypeOperation, can't determine database operation!");

            return operation;
        }
    }
}