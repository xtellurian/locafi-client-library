using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class AddActuatorDto
    {
        public string Name { get; set; }

        public int PortNo { get; set; }

        public bool ActiveState { get; set; }
    }
}
