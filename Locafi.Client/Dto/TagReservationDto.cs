using System.Collections.Generic;

namespace Locafi.Client.Data
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
