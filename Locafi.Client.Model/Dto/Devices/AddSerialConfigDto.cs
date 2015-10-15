using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class AddSerialConfigDto
    {
        public int ComPort { get; set; }

        public int BitRate { get; set; }

        public int DataBits { get; set; }

        public int Parity { get; set; }

        public int StopBits { get; set; }

        public int FlowControl { get; set; }
    }
}
