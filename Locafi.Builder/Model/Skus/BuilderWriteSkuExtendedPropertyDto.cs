using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Skus
{
    public class BuilderWriteSkuExtendedPropertyDto : WriteSkuExtendedPropertyDto
    {
        public string ExtendedPropertyName { get; set; }
    }
}
