using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.ExtendedProperties
{
    public class ExtendedPropertySummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes DataType { get; set; }

        public string TemplateType { get; set; }
    }
}
