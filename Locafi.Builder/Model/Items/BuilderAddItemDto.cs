using Locafi.Client.Model.Dto.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Items
{
    public class BuilderAddItemDto : AddItemDto
    {
        public BuilderAddItemDto()
        {
            BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>();
        }

        public string SkuName { get; set; }
        public string PlaceName { get; set; }
        public string PersonEmail { get; set; }

        public IList<BuilderWriteItemExtendedPropertyDto> BuilderItemExtendedPropertyList { get; set; }
    }
}
