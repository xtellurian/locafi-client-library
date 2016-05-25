using System;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Items
{
    public class ReadItemExtendedPropertyDto : WriteItemExtendedPropertyDto
    {
//        public Guid ExtendedPropertyId { get; set; }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public TemplateDataTypes DataType { get; set; }

        public bool IsSkuLevelProperty { get; set; }

//        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var dto = obj as ReadItemExtendedPropertyDto;
            if (dto == null) return false;
            return dto.ExtendedPropertyId == this.ExtendedPropertyId;
        }

        public override int GetHashCode()
        {
            return ExtendedPropertyId.GetHashCode();
        }
    }
}