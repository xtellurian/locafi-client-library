using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Templates
{
    public class TemplateSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor TemplateType { get; set; }
    }
}
