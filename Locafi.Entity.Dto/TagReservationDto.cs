using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class TagReservationDto
    {
        public IList<string> TagNumbers { get; set; }

        public TagReservationDto()
        {
            TagNumbers = new List<string>();
        }
    }
}
