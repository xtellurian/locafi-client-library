using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class WriteItemExtendedPropertyDto
    {


        public Guid? SkuExtendedPropertyId { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public string Value { get; set; }
    }
}