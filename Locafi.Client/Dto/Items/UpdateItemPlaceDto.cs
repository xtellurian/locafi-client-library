using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemPlaceDto
    {
        public Guid ItemId { get; set; }

        public Guid OldPlaceId { get; set; }    // why needed?

        public Guid NewPlaceId { get; set; }

        public Guid? TagId { get; set; }    // should be tagnumber, why needed?

        public DateTime DateMoved { get; set; } // why needed, should use date of request?

        public Guid MovedByUserId { get; set; } // why needed, should use person making request? (what about fixed reader telling you the persion from a tag, probably different controller/function)
    }
}