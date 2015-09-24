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
    public class TemplateExtendedPropertyDto
    {
        public Guid Id { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public Guid TemplateId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public string ExtendedPropertyDescription { get; set; }

        public bool TemplateExtendedPropertyIsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes ExtendedPropertyDataType { get; set; }

    }
}
