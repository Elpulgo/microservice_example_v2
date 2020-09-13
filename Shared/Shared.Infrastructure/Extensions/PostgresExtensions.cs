using System.Text.RegularExpressions;

namespace Shared.Infrastructure.Extensions
{
    public static class PostgresExtensions
    {
        private static Regex StartUnderScoreRegex = new Regex(@"^_+", RegexOptions.Compiled);
        private static string MatchingCharsExpression => @"([a-z0-9])([A-Z])";
        private static string ReplaceRegex => @"$1_$2";
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return $@"{StartUnderScoreRegex.Match(input).Value}{Regex.Replace(
                input,
                MatchingCharsExpression,
                ReplaceRegex)}".ToLowerInvariant();
        }
    }
}