using System.IO;

namespace Shared.Infrastructure.Constants
{

    public static class EventStreamConstants
    {
        public static string NumberOfProcessedEventsFilePath => Path.Combine(NumberOfProcessedEventsDirectory, "processed_stream_events.dat");
        public static string NumberOfProcessedEventsDirectory => Path.Combine(Directory.GetCurrentDirectory(), "data");

    }
}