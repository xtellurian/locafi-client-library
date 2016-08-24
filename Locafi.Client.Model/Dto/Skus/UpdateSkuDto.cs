using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Skus
{
    public class UpdateSkuDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; }

        public string ItemReference { get; set; }

        public string CustomPrefix { get; set; }

        public Guid TemplateId { get; set; }

        public string SkuNumber { get; set; }

        public bool IsSgtin { get; set; }

        public IList<WriteSkuExtendedPropertyDto> SkuExtendedPropertyList { get; set; }

        public UpdateSkuDto()
        {
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();
        }

        public UpdateSkuDto(SkuDetailDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            CompanyPrefix = dto.CompanyPrefix;
            ItemReference = dto.ItemReference;
            CustomPrefix = dto.CustomPrefix;
            TemplateId = dto.TemplateId;
            SkuNumber = dto.SkuNumber;

            SkuExtendedPropertyList = dto.SkuExtendedPropertyList.Select(e => new WriteSkuExtendedPropertyDto(e)).ToList();
        }

        public void ChangeTemplate(TemplateDetailDto templateDto, List<WriteSkuExtendedPropertyDto> extProps = null)
        {
            // get rid of the old extended properties
            SkuExtendedPropertyList.Clear();
            // set new template
            TemplateId = templateDto.Id;

            // populate the extended properties
            foreach(var templateProp in templateDto.TemplateExtendedPropertyList)
            {
                var providedProp = extProps?.Where(p => p.ExtendedPropertyId == templateProp.ExtendedPropertyId).FirstOrDefault();

                // use provided prop if possible, otherwise fill with random data
                if (providedProp != null)
                    SkuExtendedPropertyList.Add(providedProp);
                else
                {
                    var newProp = new WriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyId = templateProp.ExtendedPropertyId
                    };

                    switch (templateProp.ExtendedPropertyDataType)
                    {
                        case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                        case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                        case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                        case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                        case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                        case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                    }

                    newProp.IsSkuLevelProperty = true;

                    SkuExtendedPropertyList.Add(newProp);
                }
            }
        }
    }
}
