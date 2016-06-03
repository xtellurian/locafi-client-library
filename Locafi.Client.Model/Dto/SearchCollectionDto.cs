using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto
{
    public class SearchCollectionDto
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public SearchCollectionType? SearchType { get; set; }

        public IList<SearchParameterDto> SearchParameterDtos { get; set; }

        public SearchCollectionDto()
        {
            SearchParameterDtos = new List<SearchParameterDto>();
            Skip = 0;
            Take = 100;
        }
    }
}
