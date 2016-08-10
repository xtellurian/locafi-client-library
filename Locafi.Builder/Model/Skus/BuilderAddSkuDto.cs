using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Skus
{
    public class BuilderAddSkuDto : AddSkuDto
    {
        public BuilderAddSkuDto()
        {
            BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>();
        }

        public string TemplateName { get; set; }

        public IList<BuilderWriteSkuExtendedPropertyDto> BuilderSkuExtendedPropertyList { get; set; }
    }
}
