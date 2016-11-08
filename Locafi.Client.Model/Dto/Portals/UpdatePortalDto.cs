using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class UpdatePortalDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<Guid> Devices { get; set; }

        public UpdatePortalDto()
        {
            Devices = new List<Guid>();
        }
    }
}
