using System;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Skus
{
    public class WriteSkuExtendedPropertyDto
    {
        public Guid ExtendedPropertyId { get; set; }

        public bool IsSkuLevelProperty { get; set; }

        public string Value { get; set; }

        public WriteSkuExtendedPropertyDto() { }

        public WriteSkuExtendedPropertyDto(WriteSkuExtendedPropertyDto dto)
        {
            if (dto == null) return;

            var type = typeof(WriteSkuExtendedPropertyDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
