using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class TagReservationDTO
    {
        public IList<string> TagNumbers { get; set; }

        public TagReservationDTO()
        {
            TagNumbers = new List<string>();
        }
    }
}
