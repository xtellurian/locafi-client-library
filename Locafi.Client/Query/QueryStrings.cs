using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query
{
    public static class QueryStrings
    {
        public static string FilterStart => "?$filter=";

        public static string Contains(string propertyName, string sub)
        {
            if (!sub.StartsWith("'")) sub = "'" + sub;
            if (!sub.EndsWith("'")) sub = sub + "'";
            return $"contains({sub},{propertyName}) eq true";
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
}
