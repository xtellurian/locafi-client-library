using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Client.Model.Query
{
    public class SimplePlaceQuery: ISimpleRestQuery<PlaceSummaryDto>
    {
        private const string NamePropertyName = "Name";
        public SimplePlaceQuery(string name)
        {
            _queryString = $"{QueryStrings.FilterStart}{QueryStrings.Contains(name, NamePropertyName)}";
        }

        private readonly string _queryString;
        

        public string AsRestQuery()
        {
            return _queryString;
        }
    }
}
