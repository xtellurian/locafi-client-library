using System;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Skus
{
    public class ReadSkuExtendedPropertyDto
    {
        public Guid Id { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public bool ExtendedPropertyIsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes ExtendedPropertyDataType { get; set; }

        public bool IsSkuLevelProperty { get; set; }

        public string Value { get; set; }
    }
}
