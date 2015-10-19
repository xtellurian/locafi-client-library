using System;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Skus
{
    public class ReadSkuExtendedPropertyDto
    {
        public Guid Id { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public bool ExtendedPropertyIsRequired { get; set; }

        public TemplateDataTypes ExtendedPropertyDataType { get; set; }

        public bool IsSkuLevelProperty { get; set; }

        public string DefaultValue { get; set; }
    }
}
