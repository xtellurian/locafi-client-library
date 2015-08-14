using System;

namespace Locafi.Entity.Dto
{
    public class UpdateItemPlaceDto
    {
        public Guid ItemId { get; set; }

        public Guid OldPlaceId { get; set; }

        public Guid NewPlaceId { get; set; }

        public Guid? TagId { get; set; }

        public DateTime DateMoved { get; set; }

        public Guid MovedByUserId { get; set; }
    }
}