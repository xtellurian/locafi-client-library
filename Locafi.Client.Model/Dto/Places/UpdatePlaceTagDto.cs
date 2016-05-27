using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.Places
{
    public class UpdatePlaceTagDto
    {
        public Guid Id { get; set; }

        public IList<WriteTagDto> PlaceTagList { get; set; }
    }
}
