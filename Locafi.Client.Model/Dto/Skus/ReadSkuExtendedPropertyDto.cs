using System;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Dto.Skus
{
    public class ReadSkuExtendedPropertyDto : WriteSkuExtendedPropertyDto
    {
        public Guid Id { get; set; }

    //    public Guid ExtendedPropertyId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public bool ExtendedPropertyIsRequired { get; set; }

        public string ExtendedPropertyDataType { get; set; }

        //    public bool IsAllowDefaultValue { get; set; }

        //    public string DefaultValue { get; set; }

    }
}
