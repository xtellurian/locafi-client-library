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
        private static readonly Type Responsetype = typeof (PlaceSummaryDto);
        private const string FilterStart = "?$filter=";
        private const string NamePropertyName = "Name";

        public SimplePlaceQuery()
        {
            Name = "";
        }
        public SimplePlaceQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        

        public string AsRestQuery()
        {
            return $"{FilterStart}substringof('{Name}',{NamePropertyName}) eq true";
        }
    }
}
