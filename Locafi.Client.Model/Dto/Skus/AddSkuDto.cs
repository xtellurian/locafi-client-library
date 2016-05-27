using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Skus
{
    public class AddSkuDto
    {
        public AddSkuDto()
        {
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();
        }
        public string Name { get; set; }
        
        public string SkuNumber { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; } // decimal number with length eg: "0022"

        public string ItemReference { get; set; }

        public string CustomPrefix { get; set; }

        public Guid ItemTemplateId { get; set; }

        public IList<WriteSkuExtendedPropertyDto> SkuExtendedPropertyList { get;set; }

 

    }
}
