﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class InventorySummaryDto : EntityDtoBase
    {
        public InventorySummaryDto()
        {
            
        }

        public InventorySummaryDto(InventorySummaryDto dto) : base(dto)
        {
            var properties = this.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Name { get; set; }

        public Guid PlaceId { get; set; }

        public bool Complete { get; set; }
    }
}