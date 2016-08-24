using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class ResolveInventoryDto
    {
        public Guid Id { get; set; }

        public List<ResolveItemDto> FoundItemsExpected { get; set; }

        public List<ResolveItemDto> FoundItemsUnexpected { get; set; }

        public List<ResolveItemDto> MissingItems { get; set; }

        public ResolveInventoryDto()
        {
            // initialise empty arrays
            FoundItemsExpected = new List<ResolveItemDto>();
            FoundItemsUnexpected = new List<ResolveItemDto>();
            MissingItems = new List<ResolveItemDto>();
        }

        public ResolveInventoryDto(InventoryDetailDto detail, Guid? unexpectedReason = null, Guid? missingReason = null)
        {
            // initialise arrays with no reasons
            FoundItemsExpected = detail.FoundItemsExpected.Select(i => new ResolveItemDto(i.Id)).ToList();
            FoundItemsUnexpected = detail.FoundItemsUnexpected.Select(i => new ResolveItemDto(i.Id, unexpectedReason)).ToList();
            MissingItems = detail.MissingItems.Select(i => new ResolveItemDto(i.Id, missingReason)).ToList();
        }
    }
}
