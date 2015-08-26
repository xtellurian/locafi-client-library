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

        public static string Contains(string sub, string propertyName)
        {
            return $"contains('{sub}',{propertyName}) eq true";
        }

        public static string Equals(string propertyName, string value)
        {
            return $"{propertyName} eq {value}";
        }
    }
}
