using System;
using System.Globalization;
using System.IO;
using Shared.Infrastructure.Constants;

namespace Shared.Infrastructure.Events
{

    public interface IProcessedEventCountHandler
    {
        int ReadNumberOfProcessedEvents();
        void PersistsNumberOfProcessedEvents(long numberOfProcessedEvents);
    }

    public class ProcessedEventCountHandler : IProcessedEventCountHandler
    {
        public ProcessedEventCountHandler()
        {

        }

        // Note! Should throw exception if content can't be read. Shouldn't continue connecting to event stream then
        // since we can't be sure where to process from.
        public int ReadNumberOfProcessedEvents()
        {
            if (!File.Exists(EventStreamConstants.NumberOfProcessedEventsFilePath))
                return 0;

            var content = File.ReadAllText(EventStreamConstants.NumberOfProcessedEventsFilePath);
            if (!int.TryParse(content, out int numberOfProcessedEvents))
                throw new InvalidCastException($"Failed to parse content of file '{EventStreamConstants.NumberOfProcessedEventsFilePath}' to int, content: '{content}'.");

            return numberOfProcessedEvents;
        }

        public void PersistsNumberOfProcessedEvents(long numberOfProcessedEvents)
        {
            if (!Directory.Exists(EventStreamConstants.NumberOfProcessedEventsDirectory))
            {
                Directory.CreateDirectory(EventStreamConstants.NumberOfProcessedEventsDirectory);
            }

            File.WriteAllText(
                EventStreamConstants.NumberOfProcessedEventsFilePath,
                numberOfProcessedEvents.ToString(CultureInfo.InvariantCulture));
        }
    }
}