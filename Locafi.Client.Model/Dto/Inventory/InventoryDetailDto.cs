using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class InventoryDetailDto : InventorySummaryDto
    {
        public List<SkuSummaryDto> SelectedSkus { get; set; }

        public List<ItemSummaryReasonDto> FoundItemsExpected { get; set; }

        public List<ItemSummaryReasonDto> FoundItemsUnexpected { get; set; }

        public List<ItemSummaryReasonDto> MissingItems { get; set; }

        public InventoryDetailDto()
        {
            // initialise empty arrays
            FoundItemsExpected = new List<ItemSummaryReasonDto>();
            FoundItemsUnexpected = new List<ItemSummaryReasonDto>();
            MissingItems = new List<ItemSummaryReasonDto>();
        }

        public InventoryDetailDto(InventoryDetailDto dto) : base(dto)
        {
            if (dto == null) return;

            var properties = typeof(InventoryDetailDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
