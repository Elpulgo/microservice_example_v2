using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Shared.Core.Constants;
using Shared.Infrastructure.Data;
using System.Text.Json;
using EventStore.ClientAPI.Common;

namespace Shared.Infrastructure.Events
{
    public class EventStoreSubscriber<T> : IEventStoreSubscriber<T>
    {
        private readonly IEventStoreContext m_Context;
        private readonly IWriteRepository<T> m_WriteRepository;
        private readonly string m_GroupName;

        public EventStoreSubscriber(IEventStoreContext context, IWriteRepository<T> writeRepository, string groupName)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            m_WriteRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            m_GroupName = string.IsNullOrEmpty(groupName) ? throw new ArgumentNullException(nameof(groupName)) : groupName;
        }

        public async Task Subscribe()
        {
            // TODO: Should read from file which is the current position of the stream, and start from there..
            var settings = CreateSettings();
            await CreatePersistentSubscriptionIfNotExistsAsync(settings);

            await m_Context.Connection.ConnectToPersistentSubscriptionAsync(
                stream: m_Context.EventStreamName,
                m_GroupName,
                EventAppeardAction(),
                subscriptionDropped: null,
                userCredentials: null,
                bufferSize: 10,
                autoAck: false);

            Action<EventStorePersistentSubscriptionBase, ResolvedEvent> EventAppeardAction()
            {
                return async (subscription, evt) =>
                {
                    await ExecuteAction(subscription, evt);
                };
            }
        }

        private async Task ExecuteAction(EventStorePersistentSubscriptionBase subscription, ResolvedEvent evt)
        {
            var utf8EncodedData = Encoding.UTF8.GetString(evt.Event.Data);
            var entity = JsonSerializer.Deserialize<T>(utf8EncodedData);
            var operation = ParseEventTypeOperation(evt.Event.EventType);

            // DEBUG
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

                subscription.Fail(
                    @event: evt,
                    action: PersistentSubscriptionNakEventAction.Skip,
                    reason: $"Failed with operation '{operation.ToString()}' for event with data: {utf8EncodedData}, caused by {exception.Message}");

                return;
            }

            subscription.Acknowledge(evt);
        }

        private async Task CreatePersistentSubscriptionIfNotExistsAsync(PersistentSubscriptionSettings settings)
        {
            try
            {
                await m_Context.Connection.CreatePersistentSubscriptionAsync(
                    m_Context.EventStreamName,
                    m_GroupName,
                    settings,
                    new UserCredentials(
                        m_Context.Credentials.User,
                        m_Context.Credentials.Password));
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Group with name '{m_GroupName}' already exists, won't create a new persistant subscription, but attach to existing.");
            }
        }

        private static PersistentSubscriptionSettings CreateSettings()
         => PersistentSubscriptionSettings
                .Create()
                .DoNotResolveLinkTos()
                .WithNamedConsumerStrategy(SystemConsumerStrategies.DispatchToSingle)
                .StartFromBeginning()
                .Build();
        private static EventTypeOperation ParseEventTypeOperation(string input)
        {
            if (!Enum.TryParse<EventTypeOperation>(input, true, out EventTypeOperation operation))
                throw new ArgumentException($"Failed to convert '{input}' to EventTypeOperation, can't determine database operation!");

            return operation;
        }
    }
}