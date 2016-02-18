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
    public class SearchItemParameter
    {
        public string PropertyName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes DataType { get; set; }

        public string Value { get; set; }
    }
}
