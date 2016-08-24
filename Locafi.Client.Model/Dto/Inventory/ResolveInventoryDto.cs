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

    }
}
