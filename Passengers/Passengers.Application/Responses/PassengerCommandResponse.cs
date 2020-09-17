namespace Passengers.Application.Responses
{
    public class PassengerCommandResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}