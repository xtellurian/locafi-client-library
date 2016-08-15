using Locafi.Client.Model.Dto.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Places
{
    public class BuilderAddPlaceDto : AddPlaceDto
    {
        public string TemplateName { get; set; }

        public IList<BuilderWriteEntityExtendedPropertyDto> BuilderPlaceExtendedPropertyList { get; set; }

        public BuilderAddPlaceDto()
        {
            BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>();
        }
    }
}
