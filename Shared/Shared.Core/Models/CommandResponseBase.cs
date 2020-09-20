namespace Shared.Core.Models
{
    public class CommandResponseBase
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}