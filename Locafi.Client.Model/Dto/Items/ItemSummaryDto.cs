using System;
using System.Reflection;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemSummaryDto : EntityDtoBase
    {
        public ItemSummaryDto()
        {
            
        }
        public ItemSummaryDto(ItemSummaryDto dto) : base(dto)
        {
            var type = typeof(ItemSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public string Name { get; set; }
        public Guid SkuId { get; set; }
        public string SkuName { get; set; }
        public Guid PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string TagNumber { get; set; }
        public TagType TagType { get; set; }

    }
}