using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class AddPersonDto
    {
        public Guid TemplateId { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public Guid? PlaceId { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public IList<WriteTagDto> PersonTagList { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public AddPersonDto()
        {
            PersonTagList = new List<WriteTagDto>();
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }

        public AddPersonDto(TemplateDetailDto template)
        {
            TemplateId = template.Id;
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            PersonTagList = new List<WriteTagDto>();

            // popultate the extended properties
            foreach (var extProp in template.TemplateExtendedPropertyList)
            {
                var newProp = new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = extProp.ExtendedPropertyId
                };

                switch (extProp.ExtendedPropertyDataType)
                {
//                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                PersonExtendedPropertyList.Add(newProp);
            }
        }
    }
}
