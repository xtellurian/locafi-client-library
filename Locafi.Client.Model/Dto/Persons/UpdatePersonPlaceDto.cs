using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonPlaceDto
    {
        public Guid Id { get; set; }

        public Guid NewPlaceId { get; set; }

        public Guid? TagId { get; set; }
    }
}
