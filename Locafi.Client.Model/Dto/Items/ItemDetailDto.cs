using System;
using System.Collections.Generic;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemDetailDto : ItemSummaryDto
    {
        public ItemDetailDto()
        {
            
        }

        public ItemDetailDto(ItemDetailDto dto) : base(dto)
        {
            var type = typeof(ItemDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public Guid? TagId { get; set; }

        public Guid? ParentItemId { get; set; }
        public string ParentItemName { get; set; }

        public Guid? PersonId { get; set; }
        public string PersonName { get; set; }
        public string Description { get; set; }

        public IList<string> Images { get; set; }

        public IList<ReadItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }
    }
}
