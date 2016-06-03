using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portals
{

    public class UpdatePortalStatusDto
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RfidPortalStatus? Status { get; set; }
        public IList<RfidReaderStatusDto> RfidReaderStatuses { get; set; }

        public UpdatePortalStatusDto()
        {
            RfidReaderStatuses = new List<RfidReaderStatusDto>();
        }
    }
}
