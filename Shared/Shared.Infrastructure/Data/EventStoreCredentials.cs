namespace Shared.Infrastructure.Data
{
    public class EventStoreCredentials
    {
        public EventStoreCredentials(string user, string password)
        {
            User = user;
            Password = password;
        }

        public string User { get; }

        public string Password { get; }
    }
}