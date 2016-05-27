using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Portals
{
    public class UpdatePortalDto
    {
        public Guid Id { get; set; }    // guid of portal to update
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public int MaxRfidReaders { get; set; }
        public int MaxPeripheralDevices { get; set; }
        public IList<Guid> PeripheralDevices { get; set; }  // add whats in this list thats not already there, and remove whats not in this list but is already there
        public IList<Guid> RfidReaders { get; set; }
    }
}
