using System;
using System.Text.RegularExpressions;

namespace Shared.Infrastructure.Data
{
    internal static class EventStoreConnectionHelper
    {
        private static Regex ConnectionCredentialsRegex => new Regex(@"(?<=://)(?<User>[^:]*):(?<Password>[^@]*)", RegexOptions.Compiled);

        internal static void EnsureConnectionParametersIsValid(
            string connectionString,
            string eventStreamName,
            out Uri connectionStringUri)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(eventStreamName))
                throw new ArgumentNullException(nameof(eventStreamName));

            if (!Uri.TryCreate(connectionString, UriKind.Absolute, out connectionStringUri))
            {
                throw new ArgumentException($"Failed to create URI from connectionstring '{connectionString}'");
            }
        }

        internal static EventStoreCredentials CreateEventStoreCredentials(string connectionString)
        {
            var match = ConnectionCredentialsRegex.Match(connectionString);
            if (!match.Success)
                throw new ArgumentException($"Failed to parse connection credentials from {nameof(connectionString)}");

            var user = match.Groups["User"].Value;
            var password = match.Groups["Password"].Value;

            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            return new EventStoreCredentials(user, password);
        }
    }
}