using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Locafi.Client.Model.Dto
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class WriteEntityExtendedPropertyDto
    {
        public Guid ExtendedPropertyId { get; set; } // actual ID of the extended property

        public string Value { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReadEntityExtendedPropertyDto : WriteEntityExtendedPropertyDto
    {
        public Guid Id { get; set; }

        //       public Guid ExtendedPropertyId { get; set; }

        //       public Guid TemplateId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor TemplateType { get; set; }

        public string ExtendedPropertyName { get; set; }

        public string ExtendedPropertyDescription { get; set; }

        public string TemplateExtendedPropertyIsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes ExtendedPropertyDataType { get; set; }

  //      public string Value { get; set; }

    }
}
