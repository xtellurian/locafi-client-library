using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class AntennaConfigDto : EntityDtoBase
    {
        public int AntennaNo { get; set; }

        public string AntennaName { get; set; }

        public Guid InPlaceId { get; set; }

        public string InPlaceName { get; set; }

        public Guid OutPlaceId { get; set; }

        public string OutPlaceName { get; set; }

        public double TxPower { get; set; }

        public List<RssiConfigDto> RssiConfigs { get; set; }

        public bool IsEnabled { get; set; }
    }
}
