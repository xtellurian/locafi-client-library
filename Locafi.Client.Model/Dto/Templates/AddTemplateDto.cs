using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class AddTemplateDto
    {
        public AddTemplateDto()
        {
            TemplateExtendedPropertyList = new List<AddTemplateExtendedPropertyDto>();
        }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor? TemplateType { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        public IList<AddTemplateExtendedPropertyDto> TemplateExtendedPropertyList { get; set; }
    }
}
