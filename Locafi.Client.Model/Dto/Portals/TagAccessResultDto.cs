using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    class TagAccessResultDto
    {
        public bool AllowAccess { get; set; }
        public string PersonName { get; set; }
        public Guid? SecurityGroupId { get; set; }
        public string SecurityGroup { get; set; }
        public string Message { get; set; }

        public TagAccessResultDto()
        {
            AllowAccess = false;
            SecurityGroupId = null;
        }
    }
}
