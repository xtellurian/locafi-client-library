using System;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Skus
{
    public class ReadSkuExtendedPropertyDto : WriteSkuExtendedPropertyDto
    {
        public Guid Id { get; set; }

    //    public Guid ExtendedPropertyId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public bool ExtendedPropertyIsRequired { get; set; }

      //  public string ExtendedPropertyDataType { get; set; }

        public TemplateDataTypes ExtendedPropertyDataType { get; set; }

        //    public bool IsAllowDefaultValue { get; set; }

        //    public string DefaultValue { get; set; }

        public override bool Equals(object obj)
        {
            var dto = obj as ReadSkuExtendedPropertyDto;
            if (dto == null) return false;
            return dto.Id == this.Id && string.Equals(dto.ExtendedPropertyName, this.ExtendedPropertyName);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + ExtendedPropertyName.GetHashCode(); // because Legacy DB has all equal IDs for these
        }
    }
}
