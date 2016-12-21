using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class ReadSensorDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int PortNo { get; set; }

        public bool ActiveState { get; set; }
    }
}
