using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Items
{
    public class SearchItemQueryDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SearchItemQueryType QueryType;

        public IList<SearchItemParameter> QueryParameters;

        public SearchItemQueryDto()
        {
            QueryParameters = new List<SearchItemParameter>();
        }
    }
}
