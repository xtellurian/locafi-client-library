using System;

namespace Locafi.Client.Model.Dto.Skus
{
    public class WriteSkuExtendedPropertyDto
    {
        public Guid ExtendedPropertyId { get; set; }

        public bool IsAllowDefaultValue { get; set; }

        public string DefaultValue { get; set; }

    }
}
