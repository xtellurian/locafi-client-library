using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Conversion
{
    public static class ExtendedPropertyConversions
    {
        public static WriteItemExtendedPropertyDto ToWriteItem(this ReadSkuExtendedPropertyDto readSkuDto)
        {
            return new WriteItemExtendedPropertyDto
            {
                ExtendedPropertyId = readSkuDto.ExtendedPropertyId,
                SkuExtendedPropertyId = readSkuDto.Id,
                Value = readSkuDto.DefaultValue
            };
        }
    }
}
