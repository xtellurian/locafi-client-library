using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class AddAntennaDto
    {
        public string Name { get; set; }

        public int AntennaNumber { get; set; }

        public bool IsEnabled { get; set; }

        public double TxPower { get; set; }
    }
}
