using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto
{
    public abstract class DtoBase
    {
        public Guid? CreatedByUserId { get; set; }

        public string CreatedByUserFullName { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public Guid? LastModifiedByUserId { get; set; }

        public string LastModifiedByUserFullName { get; set; }

        public DateTimeOffset? DateLastModified { get; set; }
    }
}
