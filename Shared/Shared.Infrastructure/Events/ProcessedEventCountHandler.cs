using System;
using System.Globalization;
using System.IO;
using Shared.Infrastructure.Constants;

namespace Shared.Infrastructure.Events
{

    /// <Summary>
    /// Handles sequence of processed events. Persist to disk so events can be replayed
    /// if service goes down. Is implemented as a singleton, since it has a cache of the events.
    /// </Summary>
    public sealed class ProcessedEventCountHandler
    {
        private static readonly Lazy<ProcessedEventCountHandler> m_LazyProcessedEventCoundHandler
            = new Lazy<ProcessedEventCountHandler>(() => new ProcessedEventCountHandler());
        public static ProcessedEventCountHandler Instance => m_LazyProcessedEventCoundHandler.Value;
        private readonly object m_LockObject = new object();
        private int m_CurrentEventNumber;
        private ProcessedEventCountHandler()
            => m_CurrentEventNumber = ReadNumberOfProcessedEventsFromDisk();

        public int ReadNumberOfProcessedEvents()
        {
            lock (m_LockObject)
            {
                return m_CurrentEventNumber;
            }
        }

        public void PersistNumberOfProcessedEvents(long numberOfProcessedEvents)
        {
            if (!Directory.Exists(EventStreamConstants.NumberOfProcessedEventsDirectory))
            {
                Directory.CreateDirectory(EventStreamConstants.NumberOfProcessedEventsDirectory);
            }

            lock (m_LockObject)
            {
                File.WriteAllText(
                    EventStreamConstants.NumberOfProcessedEventsFilePath,
                    numberOfProcessedEvents.ToString(CultureInfo.InvariantCulture));

                m_CurrentEventNumber = (int)numberOfProcessedEvents;
            }
        }

        private int ReadNumberOfProcessedEventsFromDisk()
        {
            if (!File.Exists(EventStreamConstants.NumberOfProcessedEventsFilePath))
                return 0;

            var content = File.ReadAllText(EventStreamConstants.NumberOfProcessedEventsFilePath);
            if (!int.TryParse(content, out int numberOfProcessedEvents))
                throw new InvalidCastException($"Failed to parse content of file '{EventStreamConstants.NumberOfProcessedEventsFilePath}' to int, content: '{content}'.");

            return numberOfProcessedEvents;
        }
    }
}