using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Templates
{
    public class BuilderAddTemplateDto
    {
        public BuilderAddTemplateDto()
        {
            BuilderTemplateExtendedPropertyList = new List<BuilderAddTemplateExtendedPropertyDto>();
        }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor? TemplateType { get; set; }

        public IList<BuilderAddTemplateExtendedPropertyDto> BuilderTemplateExtendedPropertyList { get; set; }
    }
}
