using System;
using System.Collections.Generic;
using LegacyNavigatorApi.Models.Dtos;

namespace Locafi.Client.Model.Dto.Skus
{
    public class AddSkuDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; } // decimal number with length eg: "0022"

        public string ItemReference { get; set; }

        public Guid ItemTemplateId { get; set; }

        public IList<WriteSkuExtendedPropertyDto> SkuExtendedPropertyList { get;set; }

        public AddSkuDto()
        {
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();
        }

    }
}
