using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemTagDto
    {
        public Guid ItemId { get; set; }

        public Guid OldTagId { get; set; }  // why needed?

        public string NewTagId { get; set; }    // should this be tag number, also need to define tag type when updating tag

        public DateTime DateChanged { get; set; }   // why needed, should use date of request?

        public Guid ChangedByUserId { get; set; }   // why needed, should use person making request?
    }
}