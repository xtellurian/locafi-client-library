using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class UpdateAntennaDto : AddAntennaDto
    {
        public Guid? Id { get; set; }
    }
}
