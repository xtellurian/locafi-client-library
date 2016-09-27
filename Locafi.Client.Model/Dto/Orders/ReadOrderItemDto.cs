using Locafi.Client.Model.Dto.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class ReadOrderItemDto : ItemSummaryDto
    {
        public bool IsAllocated { get; set; }

        public bool IsReceived { get; set; }

        public ReadOrderItemDto(ItemSummaryDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(ItemSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
