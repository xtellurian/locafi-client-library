using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class UpdateSensorDto : AddSensorDto
    {
        public Guid? Id { get; set; }
    }
}
