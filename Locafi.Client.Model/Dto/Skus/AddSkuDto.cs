using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Skus
{
    public class AddSkuDto
    {
        public string Name { get; set; }
        
        public string SkuNumber { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; } // decimal number with length eg: "0022"

        public string ItemReference { get; set; }

        public string CustomPrefix { get; set; }

        public Guid ItemTemplateId { get; set; }

        public bool IsSgtin { get; set; }

        public IList<WriteSkuExtendedPropertyDto> SkuExtendedPropertyList { get;set; }

        public AddSkuDto()
        {
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();
        }

        public AddSkuDto(TemplateDetailDto template)
        {
            ItemTemplateId = template.Id;
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();

            // popultate the extended properties
            foreach (var extProp in template.TemplateExtendedPropertyList)
            {
                var newProp = new WriteSkuExtendedPropertyDto()
                {
                    ExtendedPropertyId = extProp.ExtendedPropertyId
                };

                switch (extProp.ExtendedPropertyDataType)
                {
                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                SkuExtendedPropertyList.Add(newProp);
            }
        }
    }
}
