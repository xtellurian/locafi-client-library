using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.ExtendedProperties
{
    public class AddExtendedPropertyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes? DataType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor? TemplateType { get; set; }
    }
}
