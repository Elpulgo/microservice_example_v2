using System;

namespace Shared.Infrastructure.Data
{
    public class EventStoreCredentials
    {
        public EventStoreCredentials(string user, string password)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentException($"'{nameof(user)}' cannot be null or empty", nameof(user));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty", nameof(password));

            User = user;
            Password = password;
        }

        public string User { get; }

        public string Password { get; }
    }
}