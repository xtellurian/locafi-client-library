using System;
using System.Collections.Generic;

namespace Locafi.Client.Data
{
    public class InventoryDto : InventoryBaseDto
    {
        public Guid Id { get; set; }
        public bool Complete { get; set; }
        //        public string SnapshotId { get; set; }
        public List<string> SnapshotIds { get; set; }
        public List<string> FoundItemsExpected { get; set; }
        public List<string> FoundItemsUnexpected { get; set; }
        public List<string> MissingItems { get; set; }
        //        public List<InventoryReasonDTO> Reasons { get; set; }
        public Dictionary<string, string> Reasons { get; set; }

        public InventoryDto()
        {
            // initialise empty arrays
            FoundItemsExpected = new List<string>();
            FoundItemsUnexpected = new List<string>();
            MissingItems = new List<string>();
            //            Reasons = new List<InventoryReasonDTO>();
            Reasons = new Dictionary<string, string>();
        }
    }
}
