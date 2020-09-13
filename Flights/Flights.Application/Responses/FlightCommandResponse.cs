namespace Flights.Application.Responses
{
    public class FlightCommandResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}