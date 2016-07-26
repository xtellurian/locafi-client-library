using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class CheckTagAccessDto
    {
        public Guid PlaceId { get; set; }
        public string TagNumber { get; set; }
    }
}
