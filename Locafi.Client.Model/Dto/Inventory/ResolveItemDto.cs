using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class ResolveItemDto
    {
        public Guid Id { get; set; }

        public Guid? ReasonId { get; set; }

        public ResolveItemDto()
        {
            ReasonId = null;
        }

        public ResolveItemDto(Guid itemId, Guid? reasonId = null)
        {
            Id = itemId;
            ReasonId = reasonId;
        }
    }
}
