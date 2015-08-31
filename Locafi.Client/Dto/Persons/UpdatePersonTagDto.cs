using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonTagDto
    {
        public Guid PersonId { get; set; }

        public string NewTagNumber { get; set; }

        public string NewTagType { get; set; } // TagType Enum
    }
}
