using Locafi.Client.Model.Dto.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Items
{
    public class BuilderWriteItemExtendedPropertyDto : WriteItemExtendedPropertyDto
    {
        public string ExtendedPropertyName { get; set; }
    }
}
