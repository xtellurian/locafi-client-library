﻿namespace Locafi.Client.Model.Query
{
    public static class QueryStrings
    {
        public static class Filter
        {
            public static string FilterStart => "$filter=";

            public static string Contains(string propertyName, string sub)
            {
                if (!sub.StartsWith("'")) sub = "'" + sub;
                if (!sub.EndsWith("'")) sub = sub + "'";
                return $"contains({propertyName},{sub}) eq true";
            }

            public static string Equals(string propertyName, string value)
            {
                return $"{propertyName} eq {value}";
            }

            public static string GreaterThan(string propertyName, string value)
            {
                return $"{propertyName} gt {value}";
            }

            public static string LessThan(string propertyName, string value)
            {
                return $"{propertyName} lt {value}";
            }
        }

        public static class Page
        {
            private static string TopLabel => "$top=";
            private static string SkipLabel => "$skip=";
            public static string TopAndSkip(int top, int skip)
            {
                return $"{TopLabel}{top}&{SkipLabel}{skip}";
            }
        }
    }
}
