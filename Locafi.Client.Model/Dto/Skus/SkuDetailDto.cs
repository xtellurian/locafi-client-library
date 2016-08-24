using System.Collections.Generic;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuDetailDto : SkuSummaryDto
    {
        public SkuDetailDto()
        {
            SkuExtendedPropertyList = new List<ReadSkuExtendedPropertyDto>();
        }

        public SkuDetailDto(SkuDetailDto dto):base(dto)
        {
            if (dto == null) return;

            var type = typeof(SkuDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public string Description { get; set; }

        public IList<string> Images { get; set; }

        public IList<ReadSkuExtendedPropertyDto> SkuExtendedPropertyList { get; set; }

    }
}
