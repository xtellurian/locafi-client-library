using System;

namespace Locafi.Client.Data
{
    public class UpdateItemTagDto
    {
        public Guid ItemId { get; set; }

        public Guid OldTagId { get; set; }

        public string NewTagId { get; set; }

        public DateTime DateChanged { get; set; }

        public Guid ChangedByUserId { get; set; }
    }
}