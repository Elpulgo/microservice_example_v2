using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Shared.Infrastructure.Data
{
    /// <Summary>
    /// To be used as Singleton, to handle the connection.
    /// EventStoreConnection is designed to be async, so one connection per application is encouraged.
    /// </Summary>
    public class EventStoreContext : IEventStoreContext, IDisposable
    {
        private readonly Uri m_ConnectionString;
        private readonly string m_EventStreamName;
        private readonly EventStoreCredentials m_EventStoreCredentials;
        private IEventStoreConnection m_EventStoreConnection;
        public IEventStoreConnection Connection => m_EventStoreConnection ?? throw new ArgumentNullException(nameof(m_EventStoreConnection));
        public string EventStreamName => m_EventStreamName;
        public EventStoreCredentials Credentials => m_EventStoreCredentials;

        public EventStoreContext(string connectionString, string eventStreamName)
        {
            EventStoreConnectionHelper.EnsureConnectionParametersIsValid(connectionString, eventStreamName, out Uri connectionStringUri);
            m_EventStoreCredentials = EventStoreConnectionHelper.CreateEventStoreCredentials(connectionString);
            m_ConnectionString = connectionStringUri;
            m_EventStreamName = eventStreamName;

            Connect().Wait();
        }

        private async Task Connect()
        {
            var connectionSettings = ConnectionSettings
                .Create()
                .DisableTls()
                .EnableVerboseLogging()
                .Build();

            var eventStoreConnection = EventStoreConnection.Create(connectionSettings, m_ConnectionString);
            await eventStoreConnection.ConnectAsync();
            m_EventStoreConnection = eventStoreConnection;
        }

        private void SubscribeToConnectionEvents()
        {
            m_EventStoreConnection.Closed += OnConnectionClosed;
            m_EventStoreConnection.Connected += OnConnectionConnected;
            m_EventStoreConnection.Disconnected += OnDisconnected;
            m_EventStoreConnection.ErrorOccurred += OnErrorOccurred;
            m_EventStoreConnection.Reconnecting += OnReconnecting;
        }

        private void OnReconnecting(object sender, ClientReconnectingEventArgs e)
         => Console.WriteLine($"Eventstream {m_EventStreamName} is reconnecting, name: '{e.Connection.ConnectionName}'");

        private void OnErrorOccurred(object sender, ClientErrorEventArgs e)
         => Console.WriteLine($"An error occurred for eventstream {m_EventStreamName}, '{e.Exception.Message}'");

        private void OnDisconnected(object sender, ClientConnectionEventArgs e)
         => Console.WriteLine($"Eventstream {m_EventStreamName} disconnected, remote endpoint: '{e.RemoteEndPoint.ToString()}'");

        private void OnConnectionConnected(object sender, ClientConnectionEventArgs e)
         => Console.WriteLine($"Eventstream {m_EventStreamName} connected to remote endpoint: '{e.RemoteEndPoint.ToString()}'");

        private void OnConnectionClosed(object sender, ClientClosedEventArgs e)
         => Console.WriteLine($"Eventstream {m_EventStreamName} closed the connection, reason: '{e.Reason}'");

        public void Dispose()
        {
            m_EventStoreConnection.Closed -= OnConnectionClosed;
            m_EventStoreConnection.Connected -= OnConnectionConnected;
            m_EventStoreConnection.Disconnected -= OnDisconnected;
            m_EventStoreConnection.ErrorOccurred -= OnErrorOccurred;
            m_EventStoreConnection.Reconnecting -= OnReconnecting;
        }
    }
}