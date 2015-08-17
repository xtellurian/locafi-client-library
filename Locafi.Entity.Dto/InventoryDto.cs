using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class InventoryDTO : InventoryBaseDTO
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

        public InventoryDTO()
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
