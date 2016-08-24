namespace Locafi.Client.Model.Query
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

            public static string ContainedIn(string sub, string propertyName)
            {
                if (!sub.StartsWith("'")) sub = "'" + sub;
                if (!sub.EndsWith("'")) sub = sub + "'";
                return $"contains({sub},{propertyName}) eq true";
            }

            public static string Equals(string propertyName, string value)
            {
                return $"{propertyName} eq {value}";
            }

            public static string NotEquals(string propertyName, string value)
            {
                return $"{propertyName} ne {value}";
            }

            public static string GreaterThan(string propertyName, string value)
            {
                return $"{propertyName} gt {value}";
            }

            public static string LessThan(string propertyName, string value)
            {
                return $"{propertyName} lt {value}";
            }

            public static string GreaterThanOrEqual(string propertyName, string value)
            {
                return $"{propertyName} ge {value}";
            }

            public static string LessThanOrEqual(string propertyName, string value)
            {
                return $"{propertyName} le {value}";
            }
        }

        public static class Page
        {
            private static string TopLabel => "$top=";
            private static string SkipLabel => "$skip=";
            public static string TopAndSkip(int top, int skip)
            {
                // If doing a top=0 just to get the count then you can't include a skip value or it generates an sql error
                if (top > 0)
                    return $"{TopLabel}{top}&{SkipLabel}{skip}";
                else if (top == 0)
                    return $"{TopLabel}{top}";
                else
                    return "";  // negative top (take) so we want to get absolutely everything, no filters
            }
        }
    }
}
